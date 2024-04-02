using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using ChannelManager.API.Services.BotHandlers;
using Service.Contracts;

namespace ChannelManager.API.Controllers;

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