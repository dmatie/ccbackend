namespace Afdb.ClientConnection.Application.Common.Exceptions;

public sealed record ExceptionContent
{
    public string Message { get; init; } = string.Empty;
    public ExceptionError[] Errors { get; init; } = [];
}

public  sealed record ExceptionError
{
    public string Field { get; init; } = string.Empty;
    public string Error { get; init; } = string.Empty;
}
