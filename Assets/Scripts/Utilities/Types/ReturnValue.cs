
public class ReturnValue<T> 
{
    public bool Result { get; }
    public T Value { get; }

    //public static ReturnValue<T> Empty => new();

    public ReturnValue()
    {
        Result = false;
        Value = default;
    }

    public ReturnValue(T value)
    {
        Result = value != null;
        Value = default;
    }

    public ReturnValue(bool result, T value)
    {
        Result = result;
        Value = value;
    }
}
