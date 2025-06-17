namespace SK.Framework;

public enum QuerySetType
{
    None,
    Single,
    Multiple,
    QueryError
}

public interface IQuerySetOne<out T>
{
    bool IsFound { get; }

    bool IsNotFound { get; }

    T Item { get; }

    Exception? Exception { get; }

    bool IsError { get; }
}

public interface IQuerySetMany<T>
{
    int Count { get; }

    bool IsFound { get; }

    bool IsNotFound { get; }

    List<T> Items { get; }

    Exception? Exception { get; }

    bool IsError { get; }
}

/// <summary>
/// Provide a read only interface for many items query result + paging information
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IQuerySetPaging<T> : IQuerySetMany<T>
{
    PagingInfo PagingInfo { get; }
}

public static class QuerySet
{
    public static IQuerySetOne<T> One<T>(T? item) where T : class
        => new QuerySetOne<T>(item);

    public static IQuerySetOne<T> OneException<T>(Exception ex) where T : class
        => new QuerySetOne<T>(ex);

    public static IQuerySetMany<T> Many<T>(List<T> items) where T : class
        => new QuerySetMany<T>(items);

    public static IQuerySetMany<T> ManyException<T>(Exception ex) where T : class
        => new QuerySetMany<T>(ex);

    public static QueryResults<T> Paging<T>(List<T> results, int total) where T : class
        => new QueryResults<T>(results, total);

    public static QueryResults<T> PagingException<T>(Exception ex) where T : class
     => new QueryResults<T>(ex);
}

/// <summary>
/// This is a class to hold query results with several nice properties
/// </summary>
/// <typeparam name="T"></typeparam>
public class QuerySet<T> where T : class
{
    /// <summary>
    /// Initializes a new instance of the <see cref="T:QuerySet"/> class.
    /// By ignoring all the optional parameters, the query set is in status of NotFound
    /// </summary>
    /// <param name="single">Set a value here if a single value is returned</param>
    /// <param name="multiple">Set a value here if a list of value is returned</param>
    public QuerySet(T? single = null, List<T>? multiple = null)
    {
        if (single == null && multiple == null)
            NotFound();
        else if (single != null)
            Found(single);
        else
            Found(multiple!);
    }

    public QuerySet(Exception ex)
    {
        NotFound();
        Error(ex);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:QuerySet"/> class.
    /// </summary>
    public QuerySet() => Type = QuerySetType.None;

    /// <summary>
    /// Gets or sets the type of the query set.
    /// </summary>
    /// <value>The type of the query set.</value>
    public QuerySetType Type { get; private set; }

    public void NotFound()
    {
        Type = QuerySetType.None;
    }

    private T? _single;

    public T? Single => _single;

    protected List<T>? _multiple;

    public List<T>? Multiple => _multiple;

    /// <summary>
    /// Query returns one result
    /// </summary>
    /// <param name="single"></param>
    public void Found(T single)
    {
        if (single == null)
        {
            NotFound();
        }
        else
        {
            Type = QuerySetType.Single;
            _single = single;
        }
    }

    /// <summary>
    /// Query returns one or more result
    /// </summary>
    /// <param name="multiple"></param>
    public void Found(List<T> multiple)
    {
        if (multiple == null)
        {
            NotFound();
        }
        else if (multiple.Count == 0)
        {
            _multiple = multiple;
            NotFound();
        }
        else
        {
            Type = QuerySetType.Multiple;
            _multiple = multiple;
        }
    }

    private Exception? _ex;

    public Exception? Exception => _ex;

    public void Error(Exception ex)
    {
        _ex = ex;
        Type = QuerySetType.QueryError;
    }

    public int Count
    {
        get
        {
            switch (Type)
            {
                case QuerySetType.None: return 0;
                case QuerySetType.Single: return 1;
                case QuerySetType.Multiple: return _multiple!.Count;
                default: return 0;
            }
        }
    }

    public bool IsFound => Type != QuerySetType.None && Type != QuerySetType.QueryError;

    public bool IsNotFound => !IsFound;

    public bool IsError => Exception is object;
}


/// <summary>
/// A query set expecting a single item
/// </summary>
/// <typeparam name="T"></typeparam>
public class QuerySetOne<T> : QuerySet<T>, IQuerySetOne<T> where T : class
{
    /// <summary>
    /// Initializes a new instance of the <see cref="T:QuerySingle"/> class.
    /// </summary>
    public QuerySetOne()
        : base()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:QuerySingle"/> class.
    /// </summary>
    public QuerySetOne(T? item)
        : base(single: item)
    {
    }

    public QuerySetOne(Exception ex)
        : base(ex)
    {
    }

    T IQuerySetOne<T>.Item => this.Single!;
}

/// <summary>
/// A query set expecting a list, array, or any IEnumerable<>
/// </summary>
/// <typeparam name="T"></typeparam>
public class QuerySetMany<T> : QuerySet<T>, IQuerySetMany<T> where T : class
{
    /// <summary>
    /// Initializes a new instance of the <see cref="T:QueryMulti"/> class.
    /// </summary>
    public QuerySetMany()
        : base()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:QueryMulti"/> class.
    /// </summary>
    public QuerySetMany(List<T> items)
        : base(multiple: items)
    {
    }

    public QuerySetMany(Exception ex)
        : base(ex)
    {
    }

    List<T> IQuerySetMany<T>.Items => this.Multiple!;
}


/// <summary>
/// A query set expecting a list, array or any IEnumerable and also paging information
/// </summary>
/// <typeparam name="T"></typeparam>
public class QuerySetPaging<T> : QuerySetMany<T>, IQuerySetPaging<T> where T : class
{
    /// <summary>
    /// Gets or sets the paging informatino.
    /// </summary>
    /// <value>The paging informatino.</value>
    public PagingInfo PagingInfo
    {
        get;
        set;
    } = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="T:QuerySetPaging"/> class.
    /// </summary>
    public QuerySetPaging()
        : base()
    {
    }

    public QuerySetPaging(List<T> items, int currentPage, int pageSize, int totalResults)
        : base(items)
    {
        PagingInfo = new PagingInfo
        {
            CurrentPage = currentPage,
            PageSize = pageSize,
            TotalResults = totalResults
        };
    }

    public QuerySetPaging(Exception ex) : this(new(), 1, 1, 0)
    {
        Error(ex);
    }
}

public class QueryResults<T> where T : class
{
    public List<T> Items { get; private set; } = new();

    public int TotalResults { get; private set; }

    public QueryResults(List<T> res, int totalResults)
    {
        Items = res;
        TotalResults = totalResults;
    }

    public QueryResults(Exception ex)
    {
        Error(ex);
    }

    public IQuerySetPaging<T> GetPagingInfo(int page, int pageSize)
        => new QuerySetPaging<T>(Items, page, pageSize, TotalResults);

    public IQuerySetPaging<T> GetPagingInfoException()
        => new QuerySetPaging<T>(_ex!);


    private Exception? _ex;

    public Exception? Exception => _ex;

    public void Error(Exception ex)
    {
        _ex = ex;
    }

    public bool IsError => Exception is object;
}