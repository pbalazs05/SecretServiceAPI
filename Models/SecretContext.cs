using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models
{
    #pragma warning disable CS1591
    public class SecretContext :DbContext
    {
        protected readonly IConfiguration Configuration;

        public SecretContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to mysql with connection string from app settings
            var connectionString = Configuration.GetConnectionString("WebApiDatabase");
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }

       // public SecretContext(DbContextOptions<SecretContext> options) : base(options) { }

        public DbSet<Secret> Secrets { get; set; } = null!;
    }
    #pragma warning disable CS1591
}
