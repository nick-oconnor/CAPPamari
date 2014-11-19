using Newtonsoft.Json;

namespace CAPPamari.Web.Models
{
    public class ApiResponse<TPayload>
    {
        [JsonConstructor]
        private ApiResponse(bool Success, string Message, TPayload Payload)
        {
            this.Success = Success;
            this.Message = Message;
            this.Payload = Payload;
        }

        public bool Success { get; private set; }
        public string Message { get; private set; }
        public TPayload Payload { get; private set; }

        public static ApiResponse<TPayload> From(bool Success, string Message, TPayload Payload)
        {
            return new ApiResponse<TPayload>(Success, Message, Payload);
        }

        public static ApiResponse<TPayload> FailureResponse(string Message, TPayload Payload)
        {
            return new ApiResponse<TPayload>(false, Message, Payload);
        }

        public static ApiResponse<TPayload> FailureResponse(string Message)
        {
            return new ApiResponse<TPayload>(false, Message, default(TPayload));
        }

        public static ApiResponse<TPayload> SuccessResponse(string Message, TPayload Payload)
        {
            return new ApiResponse<TPayload>(true, Message, Payload);
        }
    }
}