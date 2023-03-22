namespace JoggingTimeTracker.Core.Models.Results
{
    public class ResultWithData<T>
    {
        public bool Successed { get; set; }
        public string ErrorMessage { get; set; }
        public T Data { get; set; }

        private ResultWithData(bool success, string errorMessage, T data)
        {
            Successed = success;
            ErrorMessage = errorMessage;
            Data = data;
        }

        public static ResultWithData<T> Success(T data)
        {
            return new ResultWithData<T>(true, null, data);
        }

        public static ResultWithData<T> Error(string errorMessage)
        {
            return new ResultWithData<T>(false, errorMessage, default(T));
        }
    }

}
