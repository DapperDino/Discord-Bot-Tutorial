using DSharpPlus;
using DSharpPlus.Entities;
using System;
using System.Threading.Tasks;

namespace DiscordBotTutorial.Handlers.Dialogue.Steps
{
    public interface IDialogueStep
    {
        Action<DiscordMessage> OnMessageAdded { get; set; }
        IDialogueStep NextStep { get; }
        Task<bool> ProcessStep(DiscordClient client, DiscordChannel channel, DiscordUser user);
    }
}
