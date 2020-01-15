using DiscordBotTutorial.DAL;
using DiscordBotTutorial.DAL.Models.Items;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DiscordBotTutorial.Core.Services.Items
{
    public interface IItemService
    {
        Task<Item> GetItemByName(string itemName);
    }

    public class ItemService : IItemService
    {
        private readonly RPGContext _context;

        public ItemService(RPGContext context)
        {
            _context = context;
        }

        public async Task<Item> GetItemByName(string itemName)
        {
            itemName = itemName.ToLower();

            return await _context.Items
                .FirstOrDefaultAsync(x => x.Name.ToLower() == itemName).ConfigureAwait(false);
        }
    }
}
