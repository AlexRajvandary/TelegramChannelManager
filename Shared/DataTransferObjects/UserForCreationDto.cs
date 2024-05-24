using Entities.Models;

namespace Shared.DataTransferObjects;

public record UserForCreationDto(long MainChatId, long? PersonalChatId, string? BotToken, UserState State, Guid? LastEditedPostId, int UpdateId);
