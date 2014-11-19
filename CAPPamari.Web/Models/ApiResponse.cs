using Newtonsoft.Json;

namespace CAPPamari.Web.Models
{
    public class ApiResponse<TPayload>
    {
        [JsonConstructor]
        private ApiResponse(bool success, string message, TPayload payload)
        {
            Success = success;
            Message = message;
            Payload = payload;
        }

        public bool Success { get; private set; }
        public string Message { get; private set; }
        public TPayload Payload { get; private set; }

        public static ApiResponse<TPayload> From(bool success, string message, TPayload payload)
        {
            return new ApiResponse<TPayload>(success, message, payload);
        }

        public static ApiResponse<TPayload> FailureResponse(string message, TPayload payload)
        {
            return new ApiResponse<TPayload>(false, message, payload);
        }

        public static ApiResponse<TPayload> FailureResponse(string message)
        {
            return new ApiResponse<TPayload>(false, message, default(TPayload));
        }

        public static ApiResponse<TPayload> SuccessResponse(string message, TPayload payload)
        {
            return new ApiResponse<TPayload>(true, message, payload);
        }
    }
}