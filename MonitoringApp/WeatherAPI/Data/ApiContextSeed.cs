using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace WeatherAPI.Data
{
    public class ApiContextSeed
    {
        public static async Task SeedAsync(ApiContext apiContext, ILoggerFactory loggerFactory, int? retry = 0)
        {
            if (retry != null)
            {
                var retryForAvailability = retry.Value;

                try
                {
                    await apiContext.Database.MigrateAsync();
                }
                catch (Exception exception)
                {
                    if (retryForAvailability < 5)
                    {
                        retryForAvailability++;
                        var log = loggerFactory.CreateLogger<ApiContextSeed>();
                        log.LogError(exception.Message);
                        await SeedAsync(apiContext, loggerFactory, retryForAvailability);
                    }
                    throw;
                }
            }
        }
    }
}
