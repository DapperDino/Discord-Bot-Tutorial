using DiscordBotTutorial.Bots.Handlers.Dialogue;
using DiscordBotTutorial.Bots.Handlers.Dialogue.Steps;
using DiscordBotTutorial.Core.Services.Items;
using DiscordBotTutorial.DAL.Models.Items;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Threading.Tasks;

namespace DiscordBotTutorial.Bots.Commands
{
    public class ItemCommands : BaseCommandModule
    {
        private readonly IItemService _itemService;

        public ItemCommands(IItemService itemService)
        {
            _itemService = itemService;
        }

        [Command("createitem")]
        [RequireRoles(RoleCheckMode.Any, "Admin")]
        public async Task CreateItem(CommandContext ctx)
        {
            var itemPriceStep = new IntStep("How much does the item cost?", null, 1);
            var itemDescriptionStep = new TextStep("What is the item about?", itemPriceStep);
            var itemNameStep = new TextStep("What will the item be called?", itemDescriptionStep);

            var item = new Item();

            itemNameStep.OnValidResult += (result) => item.Name = result;
            itemDescriptionStep.OnValidResult += (result) => item.Description = result;
            itemPriceStep.OnValidResult += (result) => item.Price = result;

            var userChannel = await ctx.Member.CreateDmChannelAsync().ConfigureAwait(false);

            var inputDialogueHandler = new DialogueHandler(
                ctx.Client,
                userChannel,
                ctx.User,
                itemNameStep
            );

            bool succeeded = await inputDialogueHandler.ProcessDialogue().ConfigureAwait(false);

            if (!succeeded) { return; }

            await _itemService.CreateNewItemAsync(item).ConfigureAwait(false);

            await ctx.Channel.SendMessageAsync($"Item {item.Name} succesfully created!").ConfigureAwait(false);
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

            Item item = await _itemService.GetItemByNameAsync(itemName).ConfigureAwait(false);

            if (item == null)
            {
                await ctx.Channel.SendMessageAsync($"There is no item called {itemName}");
                return;
            }

            await ctx.Channel.SendMessageAsync($"Name: {item.Name}, Description: {item.Description}, Price: {item.Price}");
        }

        [Command("buy")]
        public async Task Buy(CommandContext ctx, params string[] itemNameSplit)
        {
            string itemName = string.Join(' ', itemNameSplit);

            await _itemService.PurchaseItemAsync(ctx.Member.Id, ctx.Guild.Id, itemName);
        }
    }
}
