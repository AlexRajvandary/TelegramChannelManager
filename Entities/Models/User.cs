using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class User
    {
        [Column("UserId")]
        public Guid Id { get; set; }

        [Required]
        public long ChatId { get; set; }

        [Required]
        public string? BotToken { get; set; }

        public UserState State { get; set; }

        public ICollection<Post> Posts { get; set; }
    }
}
