using System.ComponentModel.DataAnnotations;

namespace DiscordBotTutorial.DAL
{
    public abstract class Entity
    {
        [Key]
        public int Id { get; set; }
    }
}
