using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

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

        [HttpPost]
        public IActionResult CreateUser([FromBody] UserForCreationDto userForCreationDto)
        {
            if(userForCreationDto == null)
            {
                return BadRequest($"{nameof(userForCreationDto)} object is null");
            }

            var createdUser = _serviceManager.UserService.CreateUser(userForCreationDto);

            return CreatedAtRoute("UserById", new { id = createdUser.Id }, createdUser);
        }

        [HttpGet("{id:guid}")]
        public IActionResult GetUser(Guid id)
        {
            var user = _serviceManager.UserService.GetUser(id);
            return Ok(user);
        }

        [HttpGet("{chatId:long}")]
        public IActionResult GetUser(long chatId)
        {
            var user = _serviceManager.UserService.GetUserByChatId(chatId);
            return Ok(user);
        }

        [HttpPut("{id:guid}")]
        public IActionResult UpdateUser(Guid id, 
                                        [FromBody] UserForUpdateDto userForUpdateDto)
        {
            if(userForUpdateDto == null)
            {
                return BadRequest($"{nameof(userForUpdateDto)} object is null");
            }

            _serviceManager.UserService.UpdateUser(id, userForUpdateDto, trackChanges: true);
            return NoContent();
        }
    }
}
