namespace Entities.Exceptions
{
    public sealed class InvalidInputException : Exception 
    {
        public InvalidInputException()
           : base($"Invalid input.")
        {
        }
    }
}
