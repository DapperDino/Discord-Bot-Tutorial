using DiscordBotTutorial.Core.Services.Items;
using DiscordBotTutorial.Core.Services.Profiles;
using DiscordBotTutorial.DAL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordBotTutorial.Bots
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<RPGContext>(options =>
            {
                options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=RPGContext;Trusted_Connection=True;MultipleActiveResultSets=true",
                    x => x.MigrationsAssembly("DiscordBotTutorial.DAL.Migrations"));
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<IProfileService, ProfileService>();

            var serviceProvider = services.BuildServiceProvider();

            var bot = new Bot(serviceProvider);
            services.AddSingleton(bot);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

        }
    }
}
