using Entities.Models;

namespace Shared.DataTransferObjects;

public record UserForUpdateDto(long MainChatId, long? PersonalChatId, string? BotToken, UserState State, Guid? LastEditedPost);
