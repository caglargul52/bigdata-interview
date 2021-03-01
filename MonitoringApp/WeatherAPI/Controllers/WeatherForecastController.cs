using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using WeatherAPI.Entities;
using WeatherAPI.Helpers;
using WeatherAPI.Models;
using WeatherAPI.Repository.Concrete;
using WeatherAPI.Services;

namespace WeatherAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WeatherForecastController : ControllerBase
    {
        private readonly WeatherForecastService _service;

        public WeatherForecastController(WeatherForecastService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WeatherForecastModel>>> GetAllWeatherInfo()
        {
            var resultEntity = await _service.GetAllWeatherInfoAsync();

            return Ok(resultEntity);
        }

        [HttpGet("{location}")]
        public async Task<ActionResult<WeatherForecastModel>> GetWeatherInfo(string location)
        {
            var model = await _service.GetWeatherInfoAsync(location);

            if (model == null) return BadRequest();
            
            return Ok(model);
        }
    }
}
