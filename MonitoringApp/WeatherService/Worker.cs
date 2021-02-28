using System;
using System.Collections.Generic;
using System.Net;
using ColoredConsole;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NetMQ;
using NetMQ.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using WeatherService.Models;

namespace WeatherService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        private List<WeatherForecastModel> GetWeatherRecords()
        {
            var records = new List<WeatherForecastModel>();

            try
            {
                string apiUrl = ConfigurationHelper.GetWeatherAPIConfiguration();

                var client = new RestClient(apiUrl)
                {
                    Timeout = 10000
                };

                var request = new RestRequest(Method.GET);

                IRestResponse response = client.Execute(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return JsonConvert.DeserializeObject<List<WeatherForecastModel>>(response.Content);
                }
            }
            catch
            {
                // ignored
            }

            return records;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = ConfigurationHelper.GetZeroMQConfiguration();

            using var subSocket = new SubscriberSocket();

            subSocket.Connect($@"tcp://{config.Host}:{config.Port}");

            subSocket.Subscribe("weather");

            ColorConsole.WriteLine("info: ".Green(), "Subscriber socket connected.");

            var records = GetWeatherRecords();

            foreach (var item in records)
            {
                string record =
                    $@"Place Name: {item.PlaceName} - Daily Temperature: {item.DailyTemperature} - Minimum Weekly Temperature: {item.MinWeeklyTemperature} - Maximum Weekly Temperature: {item.MaxWeeklyTemperature}";

                ColorConsole.WriteLine(record.Yellow());
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    string topic = subSocket.ReceiveFrameString();
                    string message = subSocket.ReceiveFrameString();

                    var notify = JsonConvert.DeserializeObject<NotificationModel>(message);

                    ColorConsole.WriteLine("notification: ".Cyan(), $@"request: {notify.Request} - request time: {notify.RequestTime} - response: {notify.Response}");
                }
                catch
                {
                    // ignored
                }
            }
        }
    }
}
