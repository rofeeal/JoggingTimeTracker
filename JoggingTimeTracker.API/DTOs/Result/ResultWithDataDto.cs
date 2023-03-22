namespace JoggingTimeTracker.API.DTOs.Result
{
    public class ResultWithDataDto<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}