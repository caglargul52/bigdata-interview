using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using WeatherAPI.Helpers;
using WeatherAPI.Repository.Concrete;
using System.Diagnostics;
using Newtonsoft.Json;
using WeatherAPI.Entities;
using WeatherAPI.Models;

namespace WeatherAPI.Services
{
    public class WeatherForecastService
    {
        private readonly WeatherForecastRepository _repository;
        private readonly IMemoryCache _memCache;
        private readonly ZeroMQHelper _zeroMqHelper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WeatherForecastService(WeatherForecastRepository repository, IMemoryCache memCache, ZeroMQHelper zeroMqHelper, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _memCache = memCache;
            _zeroMqHelper = zeroMqHelper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<WeatherForecastModel>> GetAllWeatherInfoAsync()
        {
            var resultEntity =  await _repository.GetAllAsync(x => x.CreatedAt.Date == DateTime.Now.Date);

            return resultEntity;
        }

        public async Task<WeatherForecastModel> GetWeatherInfoAsync(string location)
        {
            var stopWatch = Stopwatch.StartNew();

            if (string.IsNullOrEmpty(location)) return null;

            var resultMemCache = _memCache.Get<WeatherForecastModel>(location);

            if (resultMemCache != null) return resultMemCache;

            location = location.ToLower();

            var resultEntity = await _repository.GetAsync(s => s.PlaceName == location && s.CreatedAt.Date == DateTime.Now.Date);

            if (resultEntity != null) return resultEntity;

            //New Record

            string locationApiUrl =
                $@"https://eu1.locationiq.com/v1/search.php?key=pk.e50be2f1da7f5a63134f0b4eaad76a60&q={location}&format=json";

            var apiLocationList = await HttpHelper.Get<List<LocationApiModel>>(locationApiUrl);

            if (apiLocationList.Count <= 0) return null;

            var apiLocation = apiLocationList[0];

            string weatherApiUrl = $@"https://api.darksky.net/forecast/f3146e0fc78b4930d41a60703c08e2ae/{apiLocation.Lat},{apiLocation.Lon}";

            var apiWeather = await HttpHelper.Get<DarkSkyApiModel>(weatherApiUrl);

            if (apiWeather == null) return null;

            var dailyTemp = (sbyte)ConverterHelper.CelsiusConverter(apiWeather.Daily.Data[0].TemperatureMax);

            var resultMinTemp = (sbyte)ConverterHelper.CelsiusConverter(apiWeather.Daily.Data.Min(x => x.TemperatureMin));

            var resultMaxTemp = (sbyte)ConverterHelper.CelsiusConverter(apiWeather.Daily.Data.Max(x => x.TemperatureMax));

            var model = new WeatherForecastModel
            {
                PlaceName = location,
                DailyTemperature = dailyTemp,
                MaxWeeklyTemperature = resultMaxTemp,
                MinWeeklyTemperature = resultMinTemp
            };

            _memCache.Set(location, model, DateTime.Now.AddSeconds(10));

            await _repository.AddAsync(model);
            await _repository.CommitAsync();

            var notify = new NotificationModel
            {
                Request = _httpContextAccessor.HttpContext.Request.Path.Value,
                Response = JsonConvert.SerializeObject(model),
                RequestTime = (short)stopWatch.Elapsed.Milliseconds
            };

            var message = JsonConvert.SerializeObject(notify);

            _zeroMqHelper.SendMessage("weather", message);

            return model;
        }
    }
}
