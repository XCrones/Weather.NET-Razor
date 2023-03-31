using Microsoft.EntityFrameworkCore;
using Weather.Domain.Entities;

namespace Weather.DAL
{
    public class AppContextDb : DbContext
    {
        public AppContextDb(DbContextOptions<AppContextDb> options) : base(options)
        {
        }

        public DbSet<Forecast> ForecastDB { get; set; }

        public DbSet<User> UserDB { get; set; }

        public DbSet<ForecastUser> ForecastUserDB { get; set; }
    }
}
