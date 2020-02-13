using DiscordBotTutorial.DAL;
using DiscordBotTutorial.DAL.Models.Profiles;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBotTutorial.Core.Services.Profiles
{
    public interface IProfileService
    {
        Task<Profile> GetOrCreateProfileAsync(ulong discordId, ulong guildId);
    }

    public class ProfileService : IProfileService
    {
        private readonly RPGContext _context;

        public ProfileService(RPGContext context)
        {
            _context = context;
        }

        public async Task<Profile> GetOrCreateProfileAsync(ulong discordId, ulong guildId)
        {
            var profile = await _context.Profiles
                .Where(x => x.GuildId == guildId)
                .FirstOrDefaultAsync(x => x.DiscordId == discordId).ConfigureAwait(false);

            if (profile != null) { return profile; }

            profile = new Profile
            {
                DiscordId = discordId,
                GuildId = guildId
            };

            _context.Add(profile);

            await _context.SaveChangesAsync().ConfigureAwait(false);

            return profile;
        }
    }
}
