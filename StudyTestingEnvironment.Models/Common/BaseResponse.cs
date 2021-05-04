namespace StudyTestingEnvironment.Models.Common
{
    public class BaseResponse
    {
        public bool IsSuccess { get; set; } = true;

        public string Message { get; set; } = string.Empty;
    }

    public class BaseResponse<TData> : BaseResponse
    {
        public TData Data { get; set; }
    }
}