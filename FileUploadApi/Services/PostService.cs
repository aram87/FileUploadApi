using FileUploadApi.Entities;
using FileUploadApi.Helpers;
using FileUploadApi.Interfaces;
using FileUploadApi.Requests;
using FileUploadApi.Response;
using FileUploadApi.Response.Models;

namespace FileUploadApi.Services
{
    public class PostService : IPostService
    {
        private readonly SocialDbContext socialDbContext;
        private readonly IWebHostEnvironment environment;

        public PostService(SocialDbContext socialDbContext, IWebHostEnvironment environment)
        {
            this.socialDbContext = socialDbContext;
            this.environment = environment;
        }

        public async Task<PostResponse> CreatePostAsync(PostRequest postRequest)
        {
            var post = new Entities.Post
            {
                UserId = postRequest.UserId,
                Description = postRequest.Description,
                Imagepath = postRequest.ImagePath,
                Ts = DateTime.Now,
                Published = true
            };

            var postEntry = await socialDbContext.Post.AddAsync(post);

            var saveResponse = await socialDbContext.SaveChangesAsync();

            if (saveResponse < 0)
            {
                return new PostResponse { Success = false, Error = "Issue while saving the post", ErrorCode = "CP01" };
            }

            var postEntity = postEntry.Entity;
            var postModel = new PostModel
            {
                Id = postEntity.Id,
                Description = postEntity.Description,
                Ts = postEntity.Ts,
                Imagepath = Path.Combine(postEntity.Imagepath),
                UserId = postEntity.UserId

            };

            return new PostResponse { Success = true, Post = postModel };

        }

        public async Task SavePostImageAsync(PostRequest postRequest)
        {
            var uniqueFileName = FileHelper.GetUniqueFileName(postRequest.Image.FileName);
            
            var uploads = Path.Combine(environment.WebRootPath, "users", "posts", postRequest.UserId.ToString());
            
            var filePath = Path.Combine(uploads, uniqueFileName);
            
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            await postRequest.Image.CopyToAsync(new FileStream(filePath, FileMode.Create));
            
            postRequest.ImagePath = filePath;

            return;
        }
    }
}
