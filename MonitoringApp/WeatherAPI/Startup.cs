using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using WeatherAPI.Data;
using WeatherAPI.Helpers;
using WeatherAPI.Repository.Concrete;
using WeatherAPI.Services;

namespace WeatherAPI
{
    public class Startup
    {
        private string ConnectionString { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            ConnectionString = Configuration.GetConnectionString("DefaultConnectionString");
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddMemoryCache();

            services.AddDbContext<ApiContext>(
            options => options.UseSqlite(ConnectionString));

            var zerMqHost = Configuration.GetConnectionString("ZeroMQ:Host");
            var zerMqPort = int.Parse(Configuration.GetConnectionString("ZeroMQ:Port"));

            services.AddSingleton(x => new ZeroMQHelper(zerMqHost, zerMqPort));

            services.AddScoped<WeatherForecastRepository, WeatherForecastRepository>();

            services.AddScoped<WeatherForecastService, WeatherForecastService>();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Version = "v1", Title = "BigData - WeatherForecast API" });
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSwagger();

            app.UseSwaggerUI();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
