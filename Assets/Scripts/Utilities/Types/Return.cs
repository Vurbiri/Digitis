
public class Return<T> where T : class
{
    public bool Result { get; }
    public T Value { get; }

    public static Return<T> Empty { get; } = new();

    private Return()
    {
        Result = false;
        Value = null;
    }

    public Return(T value)
    {
        Result = value != null;
        Value = value;
    }

    public Return(bool result, T value)
    {
        Result = result;
        Value = value;
    }
}
