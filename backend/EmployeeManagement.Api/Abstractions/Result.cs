using System.Diagnostics.CodeAnalysis;

namespace EmployeeManagement.Api.Abstractions;

public record Result
{
    protected internal Result()
    {
        Error = null;
    }
    protected internal Result(Error? error)
    {
        Error = error;
    }

    public Error? Error { get; private set; } = null;
    public bool IsSuccess => Error is null;
    public bool IsFailure => !IsSuccess;

    public static Result Success() => new();
    public static Result<TData> Success<TData>(TData data) => new(data, null);

    public static Result Failure(Error error) => new(error);
    public static Result<TData> Failure<TData>(Error error) => new(default, error);
    public static Result<TData> Failure<TData>(string message, string code) => new(default, new Error(code, message));

    public static Result<TData> Create<TData>(TData? data) =>
        data is not null ?
        Success<TData>(data) :
        Failure<TData>(Error.NullValue);
}


public record Result<TData> : Result
{
    private readonly TData? _data;

    protected internal Result(TData? value, Error? error)
        : base(error)
    {
        _data = value;
    }

    [NotNull]
    public TData Data => IsSuccess
        ? _data!
        : throw new InvalidOperationException("The value of a failure result can not be accessed.");

    public static implicit operator Result<TData>(TData? data) => Create(data);
}