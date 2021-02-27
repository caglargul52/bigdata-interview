using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherService.Model
{
    class WeatherForecastModel
    {
        public string PlaceName { get; set; }
        public sbyte DailyTemperature { get; set; }
        public sbyte MaxWeeklyTemperature { get; set; }
        public sbyte MinWeeklyTemperature { get; set; }
    }
}
