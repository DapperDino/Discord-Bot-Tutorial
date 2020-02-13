namespace DiscordBotTutorial.DAL.Models.Profiles
{
    public class Profile : Entity
    {
        public ulong DiscordId { get; set; }
        public ulong GuildId { get; set; }
        public int Xp { get; set; }
    }
}
