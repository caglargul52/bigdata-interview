using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherAPI.Models
{
    class NotificationModel
    {
        public string Request { get; set; }
        public short RequestTime { get; set; }
        public string Response { get; set; }
    }
}
