using System.Net;

namespace PizzaSalesBackend.Models.Response
{
    public class ApiResponse
    {
        public ApiResponse()
        {
            ErrorMessages = new List<string>();
        }
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; } = true;
        public bool Exist { get; set; } = false;
        public List<string> ErrorMessages { get; set; }
        public object Result { get; set; }
        public object HelperResult { get; set; }
        public int TotalCount { get; set; }
    }
}
