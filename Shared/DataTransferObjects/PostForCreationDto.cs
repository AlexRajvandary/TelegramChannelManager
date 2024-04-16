namespace Shared.DataTransferObjects;

public record PostForCreationDto(string Title, string? Content, DateTime CreatedDate);
