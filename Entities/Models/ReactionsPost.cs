using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class ReactionsPost
    {
        [Column("ReactionId")]
        public Guid Id { get; set; }

        public ReactionType Type { get; set; }

        [ForeignKey(nameof(Post))]
        public Guid PostID { get; set; }

        public Post Post { get; set; }

        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }

        public User User { get; set; }
    }
}