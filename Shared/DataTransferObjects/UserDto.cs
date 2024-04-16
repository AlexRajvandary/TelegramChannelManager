using Entities.Models;

namespace Shared.DataTransferObjects;

public record UserDto(Guid Id, long ChatId, string? BotToken, UserState State, Guid? LastEditedPost);
