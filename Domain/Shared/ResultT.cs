namespace Domain.Shared;

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    public Result(TValue? value, bool isSuccess, Error error)
        : base(isSuccess, error)
    {
        _value = value;
    }

    public TValue Value => _value!;

    public static implicit operator Result<TValue>(TValue? value) => Create(value);
}