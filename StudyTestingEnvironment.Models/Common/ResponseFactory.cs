namespace StudyTestingEnvironment.Models.Common
{
    public static class ResponseFactory
    {
        public static BaseResponse Success(string message) => new BaseResponse
        {
            IsSuccess = true,
            Message = message
        };

        public static BaseResponse<T> Success<T>(T data) => Success(data, string.Empty);

        public static BaseResponse<T> Success<T>(T data, string message) => new BaseResponse<T>
        {
            IsSuccess = true,
            Message = message,
            Data = data
        };

        public static BaseResponse Error(string message) => new BaseResponse
        {
            IsSuccess = false,
            Message = message
        };

        public static BaseResponse<T> Error<T>(string message) => Error<T>(default, message);

        public static BaseResponse<T> Error<T>(T data, string message) => new BaseResponse<T>
        {
            IsSuccess = false,
            Message = message,
            Data = data
        };
    }
}