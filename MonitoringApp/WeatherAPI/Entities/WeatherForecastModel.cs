using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherAPI.Entities
{
    public class WeatherForecastModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string PlaceName { get; set; }
        public sbyte DailyTemperature { get; set; }
        public sbyte MaxWeeklyTemperature { get; set; }
        public sbyte MinWeeklyTemperature { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
