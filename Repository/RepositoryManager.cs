using Contracts;

namespace Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryContext _repositoryContext;
        private readonly Lazy<IUserRepository> _userRepository;
        private readonly Lazy<IPostRepository> _postRepository;
        private readonly Lazy<IReactionRepository> _reactionRepository;

        public RepositoryManager(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;

            _userRepository = new Lazy<IUserRepository>(() => new UserRepository(repositoryContext));
            _postRepository = new Lazy<IPostRepository>(() => new PostRepository(repositoryContext));
            _reactionRepository = new Lazy<IReactionRepository>(() => new ReactionRepository(repositoryContext));
        }

        public IUserRepository UserRepository => _userRepository.Value;

        public IPostRepository PostRepository => _postRepository.Value;

        public IReactionRepository ReactionRepository => _reactionRepository.Value;

        public void Save() => _repositoryContext.SaveChanges();
    }
}