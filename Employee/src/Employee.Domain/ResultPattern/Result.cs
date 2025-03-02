namespace Employee.Domain.ResultPattern;

public readonly struct Result<TValue, TError>
{
    public bool IsSuccess { get; }
    public bool IsError => !IsSuccess;
    
    private readonly TValue _value;
    private readonly TError _error;

    public Result(TValue value)
    {
        IsSuccess = true;
        _value = value;
        _error = default!;
    }

    public Result(TError error)
    {
        IsSuccess = false;
        _error = error;
        _value = default!;
    }

    public static implicit operator Result<TValue, TError>(TValue value) => new(value);
    public static implicit operator Result<TValue, TError>(TError error) => new(error);

    public TResult Match<TResult>(Func<TValue, TResult> value, Func<TError, TResult> error, Func<TValue, TResult> nullValue = default)
        => IsSuccess ? (_value is null && nullValue != default) ? nullValue(_value) : value(_value) : error(_error);

    public TError GetError() => _error;
    public TValue GetValue() => _value;
}