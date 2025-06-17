namespace SK.Framework;

/// <summary>
/// Help convert .NET date time to JavaScript milliseconds and vice versa
/// </summary>
public static class JsDateTime
{
    private static readonly DateTime _epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    /// <summary>
    /// Convert .NET datetime (local or UTC) to Javascript
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static long ToJsMilliSeconds(DateTime dt)
        => (dt.ToUniversalTime().Ticks - _epoch.Ticks) / 10000;

    /// <summary>
    /// Convert from JS ms to .NET datetime UTC
    /// </summary>
    /// <param name="ms"></param>
    /// <returns></returns>
    public static DateTime ToDateTimeUtc(long ms)
        => _epoch.AddMilliseconds(ms);
}
