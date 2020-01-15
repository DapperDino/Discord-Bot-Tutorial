using DiscordBotTutorial.Bots.Handlers.Dialogue;
using DiscordBotTutorial.Bots.Handlers.Dialogue.Steps;
using DiscordBotTutorial.Core.Services.Items;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;

namespace DiscordBotTutorial.Bots.Commands
{
    public class RPGCommands : BaseCommandModule
    {
        private readonly IItemService _itemService;

        public RPGCommands(IItemService itemService)
        {
            _itemService = itemService;
        }

        [Command("iteminfo")]
        public async Task ItemInfo(CommandContext ctx)
        {
            var itemNameStep = new TextStep("What item are you looking for?", null);

            string itemName = string.Empty;

            itemNameStep.OnValidResult += (result) => itemName = result;

            var inputDialogueHandler = new DialogueHandler(
                ctx.Client,
                ctx.Channel,
                ctx.User,
                itemNameStep
            );

            bool succeeded = await inputDialogueHandler.ProcessDialogue().ConfigureAwait(false);

            if (!succeeded) { return; }

            var item = await _itemService.GetItemByName(itemName).ConfigureAwait(false);

            if (item == null)
            {
                await ctx.Channel.SendMessageAsync($"There is no item called {itemName}");
                return;
            }

            await ctx.Channel.SendMessageAsync($"Name: {item.Name}, Description: {item.Description}");
        }
    }
}
