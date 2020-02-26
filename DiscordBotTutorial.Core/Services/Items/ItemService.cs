using DiscordBotTutorial.Core.Services.Profiles;
using DiscordBotTutorial.DAL;
using DiscordBotTutorial.DAL.Models.Items;
using DiscordBotTutorial.DAL.Models.Profiles;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DiscordBotTutorial.Core.Services.Items
{
    public interface IItemService
    {
        Task CreateNewItemAsync(Item item);
        Task<Item> GetItemByNameAsync(string itemName);
        Task<bool> PurchaseItemAsync(ulong discordId, ulong guildId, string itemName);
    }

    public class ItemService : IItemService
    {
        private readonly DbContextOptions<RPGContext> _options;
        private readonly IProfileService _profileService;

        public ItemService(DbContextOptions<RPGContext> options, IProfileService profileService)
        {
            _options = options;
            _profileService = profileService;
        }

        public async Task CreateNewItemAsync(Item item)
        {
            using var context = new RPGContext(_options);

            context.Add(item);

            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<Item> GetItemByNameAsync(string itemName)
        {
            using var context = new RPGContext(_options);

            return await context.Items.FirstOrDefaultAsync(x => x.Name.ToLower() == itemName.ToLower()).ConfigureAwait(false);
        }

        public async Task<bool> PurchaseItemAsync(ulong discordId, ulong guildId, string itemName)
        {
            using var context = new RPGContext(_options);

            Item item = await GetItemByNameAsync(itemName).ConfigureAwait(false);

            if (item == null) { return false; }

            Profile profile = await _profileService.GetOrCreateProfileAsync(discordId, guildId).ConfigureAwait(false);

            if (profile.Gold < item.Price) { return false; }

            profile.Gold -= item.Price;
            profile.Items.Add(new ProfileItem
            {
                ItemId = item.Id,
                ProfileId = profile.Id
            });

            context.Profiles.Update(profile);

            await context.SaveChangesAsync().ConfigureAwait(false);

            return true;
        }
    }
}
