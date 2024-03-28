using System.ComponentModel.DataAnnotations.Schema;

namespace TelegramChannelManager.Server.Models
{
    public class User
    {
        [Column("UserId")]
        public Guid Id { get; set; }

        public long ChatId { get; set; }
    }

}
