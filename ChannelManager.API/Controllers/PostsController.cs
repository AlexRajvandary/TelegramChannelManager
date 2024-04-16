using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace ChannelManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public PostsController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpPost]
        public IActionResult CreatePost(Guid userId, [FromBody] PostForCreationDto postForCreationDto)
        {
            if (postForCreationDto == null)
            {
                return BadRequest($"{nameof(postForCreationDto)} object is null");
            }

            var createdPost = _serviceManager.PostService.CreatePost(userId, postForCreationDto, trackChanges: false);

            return CreatedAtRoute("GetPostForUser", new { id = createdPost.Id }, createdPost);
        }

        [HttpPut]
        public IActionResult UpdatePostForUser(Guid userId, Guid id, [FromBody] PostForUpdateDto postForUpdateDto)
        {
            if(postForUpdateDto == null)
            {
                return BadRequest($"{nameof(postForUpdateDto)} object is null");
            }

            _serviceManager.PostService.UpdatePostForUser(userId, id, postForUpdateDto, userTrackChanges: false, postTrackChanges: true);

            return NoContent();
        }
    }
}
