namespace lab13v9;

public class Result<T>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public string Error { get; }

    private Result(T? value, bool isSuccess, string error)
    {
        Value = value;
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result<T> Success(T value)
    {
        return new Result<T>(value, true, "");
    }

    public static Result<T> Failure(string error)
    {
        return new Result<T>(default, false, error);
    }

    public static Result<T> Failure(Exception ex)
    {
        return new Result<T>(default, false, ex.Message);
    }
}
