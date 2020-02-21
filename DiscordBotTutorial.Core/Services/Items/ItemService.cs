using DiscordBotTutorial.DAL;
using DiscordBotTutorial.DAL.Models.Items;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DiscordBotTutorial.Core.Services.Items
{
    public interface IItemService
    {
        Task CreateNewItemAsync(Item item);
        Task<Item> GetItemByNameAsync(string itemName);
    }

    public class ItemService : IItemService
    {
        private readonly DbContextOptions<RPGContext> _options;

        public ItemService(DbContextOptions<RPGContext> options)
        {
            _options = options;
        }

        public Task CreateNewItemAsync(Item item)
        {
            using var context = new RPGContext(_options);

            context.Add(item);

            return context.SaveChangesAsync();
        }

        public Task<Item> GetItemByNameAsync(string itemName)
        {
            using var context = new RPGContext(_options);

            return context.Items.FirstOrDefaultAsync(x => x.Name.ToLower() == itemName.ToLower());
        }
    }
}
