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
        private readonly RPGContext _context;

        public ItemService(RPGContext context)
        {
            _context = context;
        }

        public Task CreateNewItemAsync(Item item)
        {
            _context.Add(item);

            return _context.SaveChangesAsync();
        }

        public Task<Item> GetItemByNameAsync(string itemName)
        {
            return _context.Items.FirstOrDefaultAsync(x => x.Name.ToLower() == itemName.ToLower());
        }
    }
}
