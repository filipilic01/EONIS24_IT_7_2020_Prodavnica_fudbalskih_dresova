namespace API.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public ApiResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        private string GetDefaultMessageForStatusCode(int statusCode)
        {
            if (statusCode == 400)
            {
                return "Bad request";
            }
            else if (statusCode == 401)
            {
                return "Unauthorized";
            }
            else if (statusCode == 403)
            {
                return "Forbidden";
            }
            else if (statusCode == 404)
            {
                return "Not found";
            }
            else if (statusCode == 500)
            {
                return "Server error";
            }
            else
            {
                return null;
            }
        }
    }
}
