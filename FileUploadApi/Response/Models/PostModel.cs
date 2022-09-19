namespace FileUploadApi.Response.Models
{
    public class PostModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Description { get; set; }
        public string Imagepath { get; set; }
        public DateTime Ts { get; set; }
    }
}