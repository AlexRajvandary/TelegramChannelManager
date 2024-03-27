using System.ComponentModel.DataAnnotations.Schema;

namespace TelegramChannelManager.Server.Models
{
    public class Post
    {
        [Column("PostId")]
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }
    }

}
