using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WeatherAPI.Models;

namespace WeatherAPI.Data
{
    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options) : base(options)
        {

        }
        public DbSet<WeatherForecastModel> WeatherForecast { get; set; }
    }
}
