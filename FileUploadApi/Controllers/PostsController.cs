using FileUploadApi.Interfaces;
using FileUploadApi.Requests;
using FileUploadApi.Response;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace FileUploadApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly ILogger<PostsController> logger;
        private readonly IPostService postService;

        public PostsController(ILogger<PostsController> logger, IPostService postService)
        {
            this.logger = logger;
            this.postService = postService;
        }

        [HttpPost]
        [Route("")]
        [RequestSizeLimit(5 * 1024 * 1024)]
        public async Task<IActionResult> SubmitPost([FromForm] PostRequest postRequest)
        {
            if (postRequest == null)
            {
                return BadRequest(new PostResponse { Success = false, ErrorCode = "S01", Error = "Invalid post request" });
            }

            if (string.IsNullOrEmpty(Request.GetMultipartBoundary()))
            {
                return BadRequest(new PostResponse { Success = false, ErrorCode = "S02", Error = "Invalid post header" });
            }

            if (postRequest.Image != null)
            {
                await postService.SavePostImageAsync(postRequest);
            }

            var postResponse = await postService.CreatePostAsync(postRequest);

            if (!postResponse.Success)
            {
                return NotFound(postResponse);
            }

            return Ok(postResponse.Post);

        }
    }
}
