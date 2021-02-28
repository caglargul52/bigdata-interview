using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherAPI.Models
{
    public class Datum
    {
        public double TemperatureMin { get; set; }
        public double TemperatureMax { get; set; }
    }

    public class Daily
    {
        public List<Datum> Data { get; set; }
    }

    public class WeatherForecastApiModel
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public Daily Daily { get; set; }
    }
}
