using System.Net;

namespace Project.Data.DTO
{
    public class APIResponse
    {
        public APIResponse()
        {
            ErrorMessages = new List<string>(); 
        }
        public HttpStatusCode StatusCode { get; set; }

        public bool IsSuccess { get; set; } = true;

        public string Message { get; set; }

        public List<string> ErrorMessages { get; set; }

        public object Result { get; set; }
    }
}
