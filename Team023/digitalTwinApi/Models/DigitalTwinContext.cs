using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace digitalTwinApi.Models
{
    public class DigitalTwinContext : DbContext
    {
        public DigitalTwinContext()
        {
            
        }
        
        public DigitalTwinContext(DbContextOptions<DigitalTwinContext> options)
            : base(options)
        {    
            
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            /*if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder()     
                    .AddJsonFile("appsettings.json");   
                var Configuration = builder.Build();   
                
                optionsBuilder.UseNpgsql(Configuration["ConnectionString:DefaultConnection"]);
            }*/
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {   
            
        }

        public DbSet<Sensor> Sensors { get; set; }
        public DbSet<SensorData> SensorDatas { get; set; }
        public DbSet<Simulation> Simulations { get; set; }

    }
}