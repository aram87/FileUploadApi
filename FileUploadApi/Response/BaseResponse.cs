using System.Text.Json.Serialization;

namespace FileUploadApi.Response
{
    public class BaseResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public bool Success { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string ErrorCode { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Error { get; set; }
    }
}