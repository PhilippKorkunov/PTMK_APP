using Microsoft.EntityFrameworkCore;
using MyAppDbContext.Entities;
using MyAppDbLayer.Entities;
using System.Configuration;

namespace MyAppDbLayer
{
    public class MyAppEFDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public MyAppEFDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            ConnectionStringSettingsCollection settings = ConfigurationManager.ConnectionStrings;

            var connectionString = settings["UsersDb"].ToString();
            optionsBuilder.UseSqlServer(connectionString);

        }
    }
}
