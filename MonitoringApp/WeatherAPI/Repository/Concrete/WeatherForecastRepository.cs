using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WeatherAPI.Data;
using WeatherAPI.Models;
using WeatherAPI.Repository.Interface;

namespace WeatherAPI.Repository.Concrete
{
    public class WeatherForecastRepository : Repository<WeatherForecastModel>
    {
        public WeatherForecastRepository(ApiContext context) : base(context)
        {
        }
    }
}
