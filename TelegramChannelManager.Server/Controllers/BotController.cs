using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using TelegramChannelManager.Server.Services.UpdateHandlers;

namespace TelegramChannelManager.Server.Controllers;

[ApiController]
[Route("/")]
public class BotController : ControllerBase
{
    [HttpPost]
    [Route("/mainBot")]
    public async Task<IActionResult> UpdateFromMainBot(
        [FromBody] Update update,
        [FromServices] MainBotHandlers handleUpdateService,
        CancellationToken cancellationToken)
    {
        await handleUpdateService.HandleUpdateAsync(update, cancellationToken);
        return Ok();
    }

    [HttpPost]
    [Route("/customerBot")]
    public async Task<IActionResult> Post(
       [FromBody] Update update,
       [FromServices] CustomerBotHandlers handleUpdateService,
       CancellationToken cancellationToken)
    {
        await handleUpdateService.HandleUpdateAsync(update, cancellationToken);
        return Ok();
    }
}