namespace AkohoAspx.Services.Results
{
    public interface IOperationResult
    {
        bool IsSuccess { get; }
        string Message { get; }
    }
}
