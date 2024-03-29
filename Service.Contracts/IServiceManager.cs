namespace Service.Contracts
{
    public interface IServiceManager 
    {
        IPostService PostService { get; }
        IUserService UserService { get; }
        IReactionService ReactionService { get; }
    }
}
