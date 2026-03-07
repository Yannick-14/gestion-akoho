namespace AkohoAspx.Services.Results
{
    public sealed class OperationResult : IOperationResult
    {
        public bool IsSuccess { get; private set; }
        public string Message { get; private set; }

        private OperationResult(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message ?? string.Empty;
        }

        public static OperationResult Success(string message)
        {
            return new OperationResult(true, message);
        }

        public static OperationResult Failure(string message)
        {
            return new OperationResult(false, message);
        }
    }

    public sealed class OperationResult<T> : IOperationResult
    {
        public bool IsSuccess { get; private set; }
        public string Message { get; private set; }
        public T Value { get; private set; }

        private OperationResult(bool isSuccess, string message, T value)
        {
            IsSuccess = isSuccess;
            Message = message ?? string.Empty;
            Value = value;
        }

        public static OperationResult<T> Success(T value, string message)
        {
            return new OperationResult<T>(true, message, value);
        }

        public static OperationResult<T> Failure(string message)
        {
            return new OperationResult<T>(false, message, default(T));
        }
    }
}
