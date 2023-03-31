using Weather.DAL;
using Microsoft.EntityFrameworkCore;
using Weather.DAL.Interfaces;
using Weather.DAL.Repositories;
using Weather.Services.Implements;
using Weather.Services.Interfaces;

namespace Weather.Properties
{
    public static class Settings
    {
        public static void InitDB(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<AppContextDb>(o => o.UseNpgsql(builder.Configuration.GetConnectionString("AppContextDb"))); //Integrated Security=true;Pooling=true;
        }

        public static void InitRepositories(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IForecastRepository, ForecastRepository>();
            builder.Services.AddScoped<IForecastUserRepository, ForecastUserRepository>();
        }

        public static void InitServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IForecastService, ForecastService>();
            builder.Services.AddScoped<IOpenWeatherService, OpenWeatherService>();
            builder.Services.AddScoped<IHttpClientService, HttpClientService>();
            builder.Services.AddScoped<IForecastUserService, ForecastUserService>();
        }
    }
}
