using WeatherAPI.Data;
using WeatherAPI.Entities;
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
