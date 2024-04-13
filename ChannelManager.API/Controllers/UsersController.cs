using Entities.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

namespace ChannelManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public UsersController(IServiceManager serviceManager) 
        {
            _serviceManager = serviceManager;
        }

        [HttpGet("{id:guid}")]
        public IActionResult GetUser(Guid id)
        {
            var user = _serviceManager.UserService.GetUser(id);
            return user is null
                    ? throw new UserNotFoundException(id)
                    : (IActionResult)Ok(user);
        }

        [HttpGet("{chatId:long}")]
        public IActionResult GetUser(long chatId)
        {
            var user = _serviceManager.UserService.GetUserByChatId(chatId);
            return user is null
                    ? throw new UserNotFoundException(chatId)
                    : (IActionResult)Ok(user);
        }
    }
}
