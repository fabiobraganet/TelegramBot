
namespace ESB.Data.Context.Bots
{
    using ESB.Data.Context.Bots.Mapping;
    using ESB.Domain.Entities.Bots;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;

    /*
    dotnet ef migrations add Initial -s . -p ../../Libraries/ESB.Data -c BotsContext -o Migrations\Bots 
    dotnet ef database update
    ef migrations remove
    */

    public class BotsContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public BotsContext(IConfiguration configuration) : base()
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("SqlServerConnection"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MessageInMapping());
        }

        public DbSet<MessageIn> Message { get; set; }

    }
}
