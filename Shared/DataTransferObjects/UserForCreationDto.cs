using Entities.Models;

namespace Shared.DataTransferObjects;

public record UserForCreationDto(long ChatId, string? BotToken, UserState State);
