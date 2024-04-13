using Contracts;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;

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

        public void CreatePost(PostDTO post) => _repository.PostRepository.AddPost(post);

        public void DeletePost(PostDTO post) => _repository.PostRepository.DeletePost(post);

        public PostDTO? GetPostById(Guid postId) => _repository.PostRepository.GetPost(postId);

        public void UpdatePost(PostDTO post) => _repository.PostRepository.UpdatePost(post);
    }
}
