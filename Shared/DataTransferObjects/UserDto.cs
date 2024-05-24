using Entities.Models;

namespace Shared.DataTransferObjects;

public record UserDto(Guid Id, long MainChatId, long PersonalChatId, string BotToken, UserState State, Guid? LastEditedPostId, int UpdateId);
