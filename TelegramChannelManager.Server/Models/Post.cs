namespace TelegramChannelManager.Server.Models
{
    public class Post
    {
        public Post(string title) 
        {
            Title = title;
        }

        public Post(string title, string content, List<string> Reactions)
        {
            Title = title;
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public List<string> Reactions { get; set; }
    }
}
