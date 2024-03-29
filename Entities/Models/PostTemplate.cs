using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class PostTemplate
    {
        [Column("PostId")]
        public Guid Id { get; set; }

        public string? Title { get; set; }

        public string? Content { get; set; }

        [ForeignKey(nameof(Id))]
        public Guid UserId { get; set; }

        public User User { get; set; }

        public ICollection<ReactionType> Reactions { get; set; }
    }
}