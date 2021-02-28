using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using WeatherAPI.Models;

namespace WeatherAPI.Helpers
{
    public static class HttpHelper
    {
        public static async Task<T> Get<T>(string apiUrl) where T : class
        {
            try
            {
                var client = new RestClient(apiUrl)
                {
                    Timeout = 10000
                };

                var request = new RestRequest(Method.GET);

                IRestResponse response = await client.ExecuteAsync(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return JsonConvert.DeserializeObject<T>(response.Content);
                }
            }
            catch
            {
                // ignored
            }

            return null;
        }
    }
}
