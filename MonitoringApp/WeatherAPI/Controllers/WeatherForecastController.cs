using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using WeatherAPI.Helpers;
using WeatherAPI.Models;
using WeatherAPI.Repository.Concrete;

namespace WeatherAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WeatherForecastController : ControllerBase
    {
        private readonly WeatherForecastRepository _repository;
        private readonly IMemoryCache _memCache;
        private readonly ZeroMQHelper _zeroMqHelper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WeatherForecastController(WeatherForecastRepository repository, IMemoryCache memCache, ZeroMQHelper zeroMqHelper, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _memCache = memCache;
            _zeroMqHelper = zeroMqHelper;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<ActionResult<List<WeatherForecastModel>>> GetAllWeatherInfo()
        {
            var resultEntity = await _repository.GetAllAsync(x => x.CreatedAt.Date == DateTime.Now.Date);

            return Ok(resultEntity);
        }

        [HttpGet("{location}")]
        public async Task<ActionResult<WeatherForecastModel>> GetWeatherInfo(string location)
        {
            var stopWatch = Stopwatch.StartNew();

            if (string.IsNullOrEmpty(location)) return BadRequest();

            var resultMemCache = _memCache.Get<WeatherForecastModel>(location);

            if (resultMemCache != null) return Ok(resultMemCache);

            location = location.ToLower();

            var resultEntity = await _repository.GetAsync(s => s.PlaceName == location && s.CreatedAt.Date == DateTime.Now.Date);

            if (resultEntity != null) return Ok(resultEntity);
                
            //New Record

            string locationApiUrl =
                $@"https://eu1.locationiq.com/v1/search.php?key=pk.e50be2f1da7f5a63134f0b4eaad76a60&q={location}&format=json";

            var apiLocationList = await HttpHelper.Get<List<LocationApiModel>>(locationApiUrl);

            if (apiLocationList.Count <= 0) return BadRequest();

            var apiLocation = apiLocationList[0];

            string weatherApiUrl = $@"https://api.darksky.net/forecast/f3146e0fc78b4930d41a60703c08e2ae/{apiLocation.Lat},{apiLocation.Lon}";

            var apiWeather = await HttpHelper.Get<WeatherForecastApiModel>(weatherApiUrl);

            if (apiWeather == null) return BadRequest();

            static int CelsiusConverter(double fahrenheit)
            {
                return (int)(fahrenheit - 32) * 5 / 9;
            }

            var dailyTemp = (sbyte) CelsiusConverter(apiWeather.Daily.Data[0].TemperatureMax);
                   
            var resultMinTemp = (sbyte) CelsiusConverter(apiWeather.Daily.Data.Min(x => x.TemperatureMin));
                    
            var resultMaxTemp = (sbyte) CelsiusConverter(apiWeather.Daily.Data.Max(x => x.TemperatureMax));

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
                RequestTime = (short) stopWatch.Elapsed.Milliseconds
            };

            var message = JsonConvert.SerializeObject(notify);

            _zeroMqHelper.SendMessage("weather", message);

            return Ok(model);
        }
    }
}
