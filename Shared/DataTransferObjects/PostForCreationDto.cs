namespace Shared.DataTransferObjects;

public record PostForCreationDto(string? Title, string? Content, DateTime CreatedDate)
{
    public static PostForCreationDto CreateNewInstance() => new PostForCreationDto(null, null, DateTime.UtcNow);
}
