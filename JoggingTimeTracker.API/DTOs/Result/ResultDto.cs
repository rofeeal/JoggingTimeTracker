namespace JoggingTimeTracker.API.DTOs.Result
{
    public class ResultDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public IEnumerable<string>? Errors { get; set; }
    }
}
