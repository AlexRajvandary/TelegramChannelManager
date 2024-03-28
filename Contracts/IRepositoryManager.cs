namespace Contracts
{
    public interface IRepositoryManager
    {
        IUserRepository UserRepository { get; }

        IPostRepository PostRepository { get; }

        IReactionRepository ReactionRepository { get; }

        void Save();
    }
}