namespace ConTech.Core;

[GenerateSerializer]
public class QuerySetOneGrain<T> where T : class?
{
    public bool IsFound => Item is not null;

    public bool IsNotFound => Item is null;

    [Id(0)]
    public T? Item { get; set; }

    [Id(1)]
    public Exception? Exception { get; set; }

    public QuerySetOneGrain()
    {
    }

    public QuerySetOneGrain(T item)
    {
        Item = item;
    }

    public QuerySetOneGrain(Exception ex)
    {
        Exception = ex;
    }
}

[GenerateSerializer]
public class ResultGrain<T> where T : class
{
    /// <summary>
    /// Hold the value of the return value
    /// </summary>
    [Id(0)]
    public T Value { get; set; } = default!;

    /// <summary>
    /// Gets or sets a value indicating whether this instance is true.
    /// </summary>
    /// <value><c>true</c> if this instance is true; otherwise, <c>false</c>.</value>
    [Id(1)]
    public bool IsTrue { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is false.
    /// </summary>
    /// <value><c>true</c> if this instance is false; otherwise, <c>false</c>.</value>
    public bool IsFalse => !IsTrue;

    [Id(2)]
    public string Message { get; set; } = string.Empty;

    public ResultGrain()
    {
    }

    public ResultGrain(bool ist, T val)
    {
        IsTrue = ist;
        Value = val;
        //_exceptionObject = null;
        Message = string.Empty;
    }

    public ResultGrain(bool ist, T val, string msg)
        : this(ist, val) => Message = msg;

    public void SetTrue(T val)
    {
        IsTrue = true;
        Value = val;
    }

    public void SetFalse() => IsTrue = false;

    /// <summary>
    /// Sets the false value by passing exception
    /// </summary>
    /// <param name="e">The e.</param>
    public void SetFalse(Exception e)
    {
        IsTrue = false; ;
        Message = e.Message + " - source : " + e.Source + " - stack trace : " + e.StackTrace;
    }

    /// <summary>
    /// Return false return value.
    /// </summary>
    /// <returns></returns>
    public static ResultGrain<T> False()
    {
        var r = new ResultGrain<T>();
        r.SetFalse();

        return r;
    }

    /// <summary>
    /// Return false return value.
    /// </summary>
    /// <param name="e">The e.</param>
    /// <returns></returns>
    public static ResultGrain<T> False(Exception e)
    {
        var r = new ResultGrain<T>();
        r.SetFalse(e);

        return r;
    }

    public static ResultGrain<T> False(string errorMsg)
    {
        var r = new ResultGrain<T>();
        r.SetFalse();
        r.Message = errorMsg;

        return r;
    }

    public static ResultGrain<T> True(T value, string? successMsg = null)
    {
        var r = new ResultGrain<T>();
        r.SetTrue(value);

        if (!string.IsNullOrWhiteSpace(successMsg))
            r.Message = successMsg;

        return r;
    }
}

[GenerateSerializer]
public class ValueGrain<T> where T : struct
{
    public T Value { get; set; }

    public ValueGrain()
    {
    }

    public ValueGrain(T value)
    {
        Value = value;
    }
}