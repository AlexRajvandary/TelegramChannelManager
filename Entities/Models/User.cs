using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class User
    {
        [Column("UserId")]
        public Guid Id { get; set; }

        public long MainChatId { get; set; }

        public long? PersonalChatId { get; set; }

        public string? BotToken { get; set; }

        public Guid? LastEditedPostId { get; set; }

        public int UpdateId { get; set; }

        public UserState State { get; set; }

        public ICollection<Post> Posts { get; set; }
    }
}
