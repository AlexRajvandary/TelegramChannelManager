namespace Entities.Exceptions
{
    public sealed class UserNotFoundException : NotFoundException 
    {
        public UserNotFoundException(Guid userId) 
            : base ($"The user with id: {userId} doesn't exsist in the database.")
        { }

        public UserNotFoundException(long userChatId)
           : base($"The user with chat id: {userChatId} doesn't exsist in the database.")
        { }
    }
}
