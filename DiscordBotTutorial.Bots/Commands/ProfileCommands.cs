using DiscordBotTutorial.Core.Services.Profiles;
using DiscordBotTutorial.Core.ViewModels;
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
        private readonly IExperienceService _experienceService;

        public ProfileCommands(IProfileService profileService, IExperienceService experienceService)
        {
            _profileService = profileService;
            _experienceService = experienceService;
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
            Profile profile = await _profileService.GetOrCreateProfileAsync(memberId, ctx.Guild.Id).ConfigureAwait(false);

            DiscordMember member = ctx.Guild.Members[profile.DiscordId];

            var profileEmbed = new DiscordEmbedBuilder
            {
                Title = $"{member.DisplayName}'s Profile",
                ThumbnailUrl = member.AvatarUrl
            };

            profileEmbed.AddField("Level", profile.Level.ToString());

            await ctx.Channel.SendMessageAsync(embed: profileEmbed).ConfigureAwait(false);

            GrantXpViewModel viewModel = await _experienceService.GrantXpAsync(memberId, ctx.Guild.Id, 100).ConfigureAwait(false);

            if (!viewModel.LevelledUp) { return; }

            var levelUpEmbed = new DiscordEmbedBuilder
            {
                Title = $"{member.DisplayName} Is Now Level {viewModel.Profile.Level}",
                ThumbnailUrl = member.AvatarUrl
            };

            await ctx.Channel.SendMessageAsync(embed: levelUpEmbed).ConfigureAwait(false);
        }
    }
}
