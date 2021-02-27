using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace WeatherService
{
    internal static class ConfigurationHelper
    {
        private static IConfiguration GetConfig()
        {
            return new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory).AddJsonFile("appsettings.json").Build();
        }

        public static (string Host, int Port) GetZeroMQConfiguration()
        {
            var host = GetConfig().GetSection("ConnectionStrings").GetSection("ZeroMQ").GetSection("Host").Value;
            var port = int.Parse(GetConfig().GetSection("ConnectionStrings").GetSection("ZeroMQ").GetSection("Port").Value);
            return (host, port);
        }

        public static string GetWeatherAPIConfiguration()
        {
            var address = GetConfig().GetSection("ConnectionStrings").GetSection("WeatherAPI").GetSection("Address").Value;
            return address;
        }
    }
}
