using DiscordBotTutorial.Core.Services.Profiles;
using DiscordBotTutorial.DAL.Models.Profiles;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System.Threading.Tasks;

namespace DiscordBotTutorial.Bots.Commands
{
    public class ProfileCommands : BaseCommandModule
    {
        private readonly IProfileService _profileService;

        public ProfileCommands(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [Command("profile")]
        public async Task Profile(CommandContext ctx)
        {
            await GetProfileToDisplayAsync(ctx, ctx.Member.Id);
        }

        [Command("profile")]
        public async Task Profile(CommandContext ctx, DiscordMember member)
        {
            await GetProfileToDisplayAsync(ctx, member.Id);
        }

        private async Task GetProfileToDisplayAsync(CommandContext ctx, ulong memberId)
        {
            Profile profile = await _profileService.GetOrCreateProfileAsync(memberId, ctx.Guild.Id);

            DiscordMember member = ctx.Guild.Members[profile.DiscordId];

            var profileEmbed = new DiscordEmbedBuilder
            {
                Title = $"{member.DisplayName}'s Profile",
                ThumbnailUrl = member.AvatarUrl
            };

            profileEmbed.AddField("Xp", profile.Xp.ToString());

            await ctx.Channel.SendMessageAsync(embed: profileEmbed);
        }
    }
}
