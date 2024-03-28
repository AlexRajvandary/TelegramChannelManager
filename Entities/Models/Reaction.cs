using System.ComponentModel.DataAnnotations.Schema;

namespace TelegramChannelManager.Server.Models
{
    public class Reaction
    {
        [Column("ReactionId")]
        public Guid Id { get; set; }

        public int Quantity { get; set; }

        [ForeignKey(nameof(Post))]
        public Guid PostID { get; set; }

        public Post Post { get; set; }
    }

}
