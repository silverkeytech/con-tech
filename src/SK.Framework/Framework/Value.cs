namespace SK.Framework;

public class Value<T> where T : struct
{
    public Value(T t) => Content = t;

    public T Content { get; }

    public static implicit operator T(Value<T> w) => w.Content;
}
