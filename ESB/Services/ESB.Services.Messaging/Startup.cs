
namespace ESB.Services.Messaging
{
    using ESB.Data.Context.Bots;
    using ESB.Data.Repositories;
    using ESB.Domain.Interfaces;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public class Startup
    {
        private IHostEnvironment _hostingEnvironment;

        public Startup(IConfiguration configuration,
            IHostEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc();
            services
                .AddEntityFrameworkSqlServer()
                .AddDbContext<BotsContext>(Option 
                    => Option.UseSqlServer(Configuration.GetConnectionString("SqlServerConnection"), 
                        x => x.MigrationsAssembly("ESB.Data.Migrations.Bots")));

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapHealthChecks("/health");

                endpoints.MapGrpcService<BotMessageService>();

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("");
                });
            });
        }
    }
}
