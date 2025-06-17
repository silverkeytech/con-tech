namespace SK.Framework;

/// <summary>
/// This is a simple return value structure to make passing return value easier.
/// </summary>
/// <typeparam name="T"></typeparam>
public struct Result<T>
{
    public Result(bool ist, T val)
    {
        IsTrue = ist;
        Value = val;
        _exceptionObject = null;
        Message = string.Empty;
    }

    public Result(bool ist, T val, string msg)
        : this(ist, val) => Message = msg;

    /// <summary>
    /// Hold the value of the return value
    /// </summary>
    public T Value { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is true.
    /// </summary>
    /// <value><c>true</c> if this instance is true; otherwise, <c>false</c>.</value>
    public bool IsTrue { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is false.
    /// </summary>
    /// <value><c>true</c> if this instance is false; otherwise, <c>false</c>.</value>
    public bool IsFalse => !IsTrue;

    public string Message { get; set; }

    private Exception _exceptionObject;

    public Exception ExceptionObject
    {
        get
        {
            //This is deliberate because sometime people do not always pass exception when the false value is set. So
            //if somebody tries to retrieve this Exception object while the result wasn't set in the first place, we must give them valid
            //exception object although it won't contains much information about stack trace
            if (_exceptionObject == null && IsFalse)
                _exceptionObject = new ApplicationException(Message ?? "Exception Message is Null");

            return _exceptionObject;
        }

        set
        {
            _exceptionObject = value;
        }
    }

    /// <summary>
    /// Sets the true value
    /// </summary>
    /// <param name="value">The value.</param>
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
        ExceptionObject = e;
    }

    /// <summary>
    /// Return false return value.
    /// </summary>
    /// <returns></returns>
    public static Result<T> False()
    {
        var r = new Result<T>();
        r.SetFalse();

        return r;
    }

    /// <summary>
    /// Return false return value.
    /// </summary>
    /// <param name="e">The e.</param>
    /// <returns></returns>
    public static Result<T> False(Exception e)
    {
        var r = new Result<T>();
        r.SetFalse(e);

        return r;
    }

    public static Result<T> False(string errorMsg)
    {
        var r = new Result<T>();
        r.SetFalse();
        r.Message = errorMsg;

        return r;
    }

    public static Result<T> True(T value, string? successMsg = null)
    {
        var r = new Result<T>();
        r.SetTrue(value);

        if (!string.IsNullOrWhiteSpace(successMsg))
            r.Message = successMsg;

        return r;
    }

    public static implicit operator T(Result<T> returnValue) => returnValue.Value;
}

public struct Result
{
    public Result(bool ist)
    {
        IsTrue = ist;
        Message = string.Empty;
    }

    public Result(bool ist, string msg)
        : this(ist) => Message = msg;

    /// <summary>
    /// Gets or sets a value indicating whether this instance is true.
    /// </summary>
    /// <value><c>true</c> if this instance is true; otherwise, <c>false</c>.</value>
    public bool IsTrue { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is false.
    /// </summary>
    /// <value><c>true</c> if this instance is false; otherwise, <c>false</c>.</value>
    public bool IsFalse => !IsTrue;

    public string Message { get; set; }

    /// <summary>
    /// Sets the true value
    /// </summary>
    /// <param name="value">The value.</param>
    public void SetTrue()
    {
        IsTrue = true;
    }

    public void SetFalse() => IsTrue = false;

    /// <summary>
    /// Sets the false value by passing exception
    /// </summary>
    /// <param name="e">The e.</param>
    public void SetFalse(Exception e)
    {
        IsTrue = false; ;
        Message = e.Message /*+ " - source : " + e.Source + " - stack trace : " + e.StackTrace*/;
    }

    /// <summary>
    /// Return false return value.
    /// </summary>
    /// <returns></returns>
    public static Result False()
    {
        var r = new Result();
        r.SetFalse();

        return r;
    }

    /// <summary>
    /// Return false return value.
    /// </summary>
    /// <param name="e">The e.</param>
    /// <returns></returns>
    public static Result False(Exception e)
    {
        var r = new Result();
        r.SetFalse(e);

        return r;
    }

    public static Result False(string errorMsg)
    {
        var r = new Result();
        r.SetFalse();
        r.Message = errorMsg;

        return r;
    }

    public static Result True(string? successMsg = null)
    {
        var r = new Result();
        r.SetTrue();

        if (!string.IsNullOrWhiteSpace(successMsg))
            r.Message = successMsg;

        return r;
    }
}
