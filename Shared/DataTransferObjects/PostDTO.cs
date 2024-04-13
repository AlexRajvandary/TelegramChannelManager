namespace Shared.DataTransferObjects;

public record PostDTO(Guid Id, string Title, string Content, DateTime CreatedDate);