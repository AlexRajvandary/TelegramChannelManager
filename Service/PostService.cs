using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service
{
    public class PostService : IPostService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public PostService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public PostDto CreatePost(Guid userId, PostForCreationDto postForCreation, bool trackChanges)
        {
            var user = _repository.UserRepository.GetUser(userId, trackChanges);
            if (user is null)
            {
                throw new UserNotFoundException(userId);
            }

            var postEntity = _mapper.Map<Post>(postForCreation);
            _repository.PostRepository.CreatePostForUser(userId, postEntity);
            _repository.Save();
            var postToReturn = _mapper.Map<PostDto>(postEntity);
            return postToReturn;
        }

        public void DeletePost(Guid userId, Guid postId)
        {
            throw new NotImplementedException();
        }

        public PostDto? GetPost(Guid userId, Guid postId, bool trackChanges)
        {
            var user = _repository.UserRepository.GetUser(userId, trackChanges);
            if (user is null)
            {
                throw new UserNotFoundException(userId);
            }

            var post = _repository.PostRepository.GetPost(userId, postId, trackChanges);
            if (post is null)
            {
                throw new PostNotFoundException(postId);
            }

            var postDto = _mapper.Map<PostDto>(post);
            return postDto;
        }

        public IEnumerable<PostDto>? GetPosts(Guid userId, bool trackChanges)
        {
            var user = _repository.UserRepository.GetUser(userId, trackChanges);
            if (user is null)
            {
                throw new UserNotFoundException(userId);
            }

            var postsFromDb = _repository.PostRepository.GetPosts(userId, trackChanges);
            var postDtos = _mapper.Map<IEnumerable<PostDto>>(postsFromDb);
            return postDtos;
        }

        public void UpdatePostForUser(Guid userId,
                               Guid postId,
                               PostForUpdateDto postForUpdate,
                               bool userTrackChanges,
                               bool postTrackChanges)
        {
            var user = _repository.UserRepository.GetUser(userId, userTrackChanges);
            if (user is null)
            {
                throw new UserNotFoundException(userId);
            }

            var postFromDb = _repository.PostRepository.GetPost(userId, postId, postTrackChanges);
            if (postFromDb is null)
            {
                throw new PostNotFoundException(userId);
            }

            _mapper.Map(postForUpdate, postFromDb);
            _repository.Save();
        }
    }
}
