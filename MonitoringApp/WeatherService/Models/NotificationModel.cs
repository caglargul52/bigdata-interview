using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherService.Models
{
    class NotificationModel
    {
        public string Request { get; set; }
        public short RequestTime { get; set; }
        public string Response { get; set; }
    }
}
