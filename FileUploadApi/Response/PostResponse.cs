using FileUploadApi.Response.Models;

namespace FileUploadApi.Response
{
    public class PostResponse : BaseResponse
    {
        public PostModel Post { get; set; }
    }
}
