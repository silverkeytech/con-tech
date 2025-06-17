namespace SK.Framework;

public static class IteratorExtension
{
    /// <summary>
    /// This extension method apply the given lambda to every single item in the list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="self"></param>
    /// <param name="func"></param>
    public static void ForEach<T>(this IEnumerable<T> self, Action<T> func)
    {
        foreach (var i in self)
            func(i);
    }

    public static bool IsAny<T>(this IEnumerable<T> coll, Func<T, bool>? predicate = null)
    {
        if (coll == null)
            return false;

        if (predicate != null)
            return coll.Any(predicate);
        else
            return coll.Any();
    }
}
