using Entities.Models;

namespace Shared.DataTransferObjects;

public record UserForUpdateDto(long ChatId, string? BotToken, UserState State, IEnumerable<PostForCreationDto> Posts);
