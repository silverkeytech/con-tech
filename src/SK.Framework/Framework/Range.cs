namespace SK.Framework;

public struct Range<T> where T : struct
{
    public T? From { get; set; }

    public T? To { get; set; }

    public Range(T? from, T? to)
    {
        From = from;
        To = to;
    }

    public bool All => From.HasValue && To.HasValue;

    public bool FromOnly => From.HasValue && !To.HasValue;

    public bool ToOnly => !From.HasValue && To.HasValue;

    public bool None => !From.HasValue && !To.HasValue;
}
