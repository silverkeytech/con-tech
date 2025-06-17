namespace SK.Framework;

/// <summary>
/// This structure to indicates no value (to be used with generics). We need to resort to this because System.Void does not work.
/// </summary>
public class None
{
    public static None Instance => new None();

    public static Result<None> True() => Result<None>.True(Instance);

    public static Result<None> False(Exception ex) => Result<None>.False(ex);
}

/// <summary>
/// This structure to indicates no value (to be used with generics). We need to resort to this because System.Void does not work.
/// </summary>
public class Nothing : None
{
    /// <summary>
    /// Return an instance of Nothing
    /// </summary>
    public static new Nothing Instance => new Nothing();

    /// <summary>
    /// Return an empty list of Nothing
    /// </summary>
    public static List<Nothing> EmptyList => new List<Nothing>();

    public static List<Nothing> OneItemList => new List<Nothing>() { Nothing.Instance };
}
