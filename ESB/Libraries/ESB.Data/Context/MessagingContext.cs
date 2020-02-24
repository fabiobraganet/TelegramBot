
namespace ESB.Data.Context
{
    using ESB.Domain.Entities.Bots;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;

    /*
    dotnet ef migrations add Initial -s . -p ../../Libraries/ESB.Data -c MessagingContext -o Migrations\Messagin 
    dotnet ef database update
    ef migrations remove
    */

    public class MessagingContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public MessagingContext(IConfiguration configuration) : base()
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("SqlServerConnection"));
            //optionsBuilder.UseNpgsql(_configuration.GetConnectionString("Postgresql"), npgOptions => npgOptions.UseNetTopologySuite());
        }

        public DbSet<Message> Message { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.HasPostgresExtension("postgis");
        }
    }
}
