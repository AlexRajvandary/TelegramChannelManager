namespace Shared.DataTransferObjects;

public record PostDto(Guid Id, string Title, string Content, DateTime CreatedDate);