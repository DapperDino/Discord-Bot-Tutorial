using DiscordBotTutorial.DAL;
using DiscordBotTutorial.DAL.Models.Items;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DiscordBotTutorial.Bots.Commands
{
    public class TeamCommands : BaseCommandModule
    {
        private readonly RPGContext _context;

        public TeamCommands(RPGContext context)
        {
            _context = context;
        }

        [Command("join")]
        public async Task Join(CommandContext ctx)
        {
            var joinEmbed = new DiscordEmbedBuilder
            {
                Title = "Would you like to join?",
                ThumbnailUrl = ctx.Client.CurrentUser.AvatarUrl,
                Color = DiscordColor.Green
            };

            var joinMessage = await ctx.Channel.SendMessageAsync(embed: joinEmbed).ConfigureAwait(false);

            var thumbsUpEmoji = DiscordEmoji.FromName(ctx.Client, ":+1:");
            var thumbsDownEmoji = DiscordEmoji.FromName(ctx.Client, ":-1:");

            await joinMessage.CreateReactionAsync(thumbsUpEmoji).ConfigureAwait(false);
            await joinMessage.CreateReactionAsync(thumbsDownEmoji).ConfigureAwait(false);

            var interactivity = ctx.Client.GetInteractivity();

            var reactionResult = await interactivity.WaitForReactionAsync(
                x => x.Message == joinMessage &&
                x.User == ctx.User &&
                (x.Emoji == thumbsUpEmoji || x.Emoji == thumbsDownEmoji)).ConfigureAwait(false);

            if (reactionResult.Result.Emoji == thumbsUpEmoji)
            {
                var role = ctx.Guild.GetRole(649318238165139495);
                await ctx.Member.GrantRoleAsync(role).ConfigureAwait(false);
            }

            await joinMessage.DeleteAsync().ConfigureAwait(false);
        }

        [Command("additem")]
        public async Task AddItem(CommandContext ctx, string name)
        {
            await _context.Items.AddAsync(new Item { Name = name, Description = "Test Description" }).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        [Command("item")]
        public async Task Item(CommandContext ctx, string name)
        {
            var items = await _context.Items.ToListAsync().ConfigureAwait(false);
        }
    }
}
