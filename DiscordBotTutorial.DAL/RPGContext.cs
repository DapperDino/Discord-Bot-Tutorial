using DiscordBotTutorial.DAL.Models.Items;
using Microsoft.EntityFrameworkCore;

namespace DiscordBotTutorial.DAL
{
    public class RPGContext : DbContext
    {
        public RPGContext(DbContextOptions<RPGContext> options) : base(options) { }

        public DbSet<Item> Items { get; set; }
    }
}
