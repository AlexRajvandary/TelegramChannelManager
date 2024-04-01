using Contracts;
using Entities.Models;
using Service.Contracts;

namespace Service
{
    public class PostService : IPostService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;

        public PostService(IRepositoryManager repository, ILoggerManager logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public void CreatePost(Post post) => _repository.PostRepository.AddPost(post);

        public void DeletePost(Post post) => _repository.PostRepository.DeletePost(post);

        public Post? GetPostById(Guid postId) => _repository.PostRepository.GetPost(postId);

        public void UpdatePost(Post post) => _repository.PostRepository.UpdatePost(post);
    }
}
