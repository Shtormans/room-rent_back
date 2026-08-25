namespace Domain.Shared;

public class Error
{
    public static readonly Error None = new(string.Empty);
    public static readonly Error NullValue = new("Error.NullValue");

    public Error(string code)
    {
        Code = code;
    }

    public string Code { get; }

    public static implicit operator string(Error error) => error.Code;
}