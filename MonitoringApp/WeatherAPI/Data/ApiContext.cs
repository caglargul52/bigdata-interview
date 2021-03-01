using Microsoft.EntityFrameworkCore;
using WeatherAPI.Entities;

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
