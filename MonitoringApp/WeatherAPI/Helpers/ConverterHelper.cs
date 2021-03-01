using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherAPI.Helpers
{
    public static class ConverterHelper
    {
        public static int CelsiusConverter(double fahrenheit)
        {
            return (int)(fahrenheit - 32) * 5 / 9;
        }
    }
}
