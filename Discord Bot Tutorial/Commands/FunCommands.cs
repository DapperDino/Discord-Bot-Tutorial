using DiscordBotTutorial.Attributes;
using DiscordBotTutorial.Handlers.Dialogue;
using DiscordBotTutorial.Handlers.Dialogue.Steps;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBotTutorial.Commands
{
    public class FunCommands : BaseCommandModule
    {
        [Command("ping")]
        [Description("Returns pong")]
        [RequireCategories(ChannelCheckMode.Any, "Text Channels")]
        public async Task Ping(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync("Pong").ConfigureAwait(false);
        }

        [Command("add")]
        [Description("Adds two numbers together")]
        [RequireRoles(RoleCheckMode.Any, "Moderator", "Owner")]
        public async Task Add(CommandContext ctx,
            [Description("First Number")] int numberOne,
            [Description("Second Number")]int numberTwo)
        {
            await ctx.Channel
                .SendMessageAsync((numberOne + numberTwo).ToString())
                .ConfigureAwait(false);
        }

        [Command("respondmessage")]
        public async Task RespondMessage(CommandContext ctx)
        {
            var interactivity = ctx.Client.GetInteractivity();

            var message = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel).ConfigureAwait(false);

            await ctx.Channel.SendMessageAsync(message.Result.Content);
        }

        [Command("respondreaction")]
        public async Task RespondReaction(CommandContext ctx)
        {
            var interactivity = ctx.Client.GetInteractivity();

            var message = await interactivity.WaitForReactionAsync(x => x.Channel == ctx.Channel).ConfigureAwait(false);

            await ctx.Channel.SendMessageAsync(message.Result.Emoji);
        }

        [Command("poll")]
        public async Task Poll(CommandContext ctx, TimeSpan duration, params DiscordEmoji[] emojiOptions)
        {
            var interactivity = ctx.Client.GetInteractivity();
            var options = emojiOptions.Select(x => x.ToString());

            var pollEmbed = new DiscordEmbedBuilder
            {
                Title = "Poll",
                Description = string.Join(" ", options)
            };

            var pollMessage = await ctx.Channel.SendMessageAsync(embed: pollEmbed).ConfigureAwait(false);

            foreach (var option in emojiOptions)
            {
                await pollMessage.CreateReactionAsync(option).ConfigureAwait(false);
            }

            var result = await interactivity.CollectReactionsAsync(pollMessage, duration).ConfigureAwait(false);
            var distinctResult = result.Distinct();

            var results = distinctResult.Select(x => $"{x.Emoji}: {x.Total}");

            await ctx.Channel.SendMessageAsync(string.Join("\n", results)).ConfigureAwait(false);
        }

        [Command("dialogue")]
        public async Task Dialogue(CommandContext ctx)
        {
            var inputStep = new TextStep("Enter something interesting!", null, 10);
            var funnyStep = new IntStep("Haha, funny", null, maxValue: 100);

            string input = string.Empty;
            int value = 0;

            inputStep.OnValidResult += (result) =>
            {
                input = result;

                if (result == "something interesting")
                {
                    inputStep.SetNextStep(funnyStep);
                }
            };

            funnyStep.OnValidResult += (result) => value = result;

            var userChannel = await ctx.Member.CreateDmChannelAsync().ConfigureAwait(false);

            var inputDialogueHandler = new DialogueHandler(
                ctx.Client,
                userChannel,
                ctx.User,
                inputStep
            );

            bool succeeded = await inputDialogueHandler.ProcessDialogue().ConfigureAwait(false);

            if (!succeeded) { return; }

            await ctx.Channel.SendMessageAsync(input).ConfigureAwait(false);

            await ctx.Channel.SendMessageAsync(value.ToString()).ConfigureAwait(false);
        }

        [Command("emojidialogue")]
        public async Task EmojiDialogue(CommandContext ctx)
        {
            var yesStep = new TextStep("You chose yes", null);
            var noStep = new IntStep("You chose no", null);

            var emojiStep = new ReactionStep("Yes Or No?", new Dictionary<DiscordEmoji, ReactionStepData>
            {
                { DiscordEmoji.FromName(ctx.Client, ":thumbsup:"), new ReactionStepData { Content = "This means yes", NextStep = yesStep } },
                { DiscordEmoji.FromName(ctx.Client, ":thumbsdown:"), new ReactionStepData { Content = "This means no", NextStep = noStep } }
            });

            var userChannel = await ctx.Member.CreateDmChannelAsync().ConfigureAwait(false);

            var inputDialogueHandler = new DialogueHandler(
                ctx.Client,
                userChannel,
                ctx.User,
                emojiStep
            );

            bool succeeded = await inputDialogueHandler.ProcessDialogue().ConfigureAwait(false);

            if (!succeeded) { return; }
        }
    }
}
