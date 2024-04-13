using AutoMapper;
using Contracts;
using Service.Contracts;

namespace Service
{
    public sealed class ServiceManager : IServiceManager
    {
        private readonly Lazy<IUserService> _userService;
        private readonly Lazy<IPostService> _postService;
        private readonly Lazy<IReactionService> _reactionService;

        public ServiceManager(IRepositoryManager repositoryManager, ILoggerManager logger, IMapper mapper)
        {
            _userService = new Lazy<IUserService>(() => new UserService(repositoryManager, logger, mapper));
            _postService = new Lazy<IPostService>(() => new PostService(repositoryManager, logger, mapper));
            _reactionService = new Lazy<IReactionService>(() => new ReactionService(repositoryManager, logger));
        }
        
        public IPostService PostService => _postService.Value;

        public IUserService UserService => _userService.Value;

        public IReactionService ReactionService => _reactionService.Value;
    }
}
