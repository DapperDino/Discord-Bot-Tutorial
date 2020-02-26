namespace DiscordBotTutorial.DAL.Models.Items
{
    public class Item : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
    }
}
