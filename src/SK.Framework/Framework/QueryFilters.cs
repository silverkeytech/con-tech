using SD.LLBLGen.Pro.LinqSupportClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ConTech.Data.EntityClasses;
using ConTech.Data.Linq;

namespace SK.Framework;

public interface IFilter<T>
{
    IQueryable<T> Filter(IQueryable<T> query, LinqMetaData? meta = null);
}

public interface IFilter
{
    IPredicate Filter();
}

public interface ISort<T>
{
    IOrderedQueryable<T> Sort(IQueryable<T> query);
}

public interface IPrefetch
{
    SD.LLBLGen.Pro.ORMSupportClasses.IPrefetchPath2 Get();
}

public class QueryCondition<T>
{
    public IFilter<T>? Filter { get; set; }

    public QueryCondition()
    {

    }

    public QueryCondition(IFilter<T> f)
    {
        Filter = f;
    }
}

public class QueryConditionWithPrefetch<T> : QueryCondition<T>
{
    public IPrefetch Prefetch { get; set; }

    public QueryConditionWithPrefetch(IFilter<T> f, IPrefetch p) : base(f)
    {
        Prefetch = p;
    }
}

public class PagingQueryConditionPrefetched<T> : QueryConditionWithPrefetch<T>
{
    public int Page { get; set; }

    public int PageSize { get; set; }

    public PagingQueryConditionPrefetched(IFilter<T> f, IPrefetch p, int page, int pageSize) : base(f, p)
    {
        Page = page;
        PageSize = pageSize;
    }
}

public class SortedPagingQueryConditionPrefetched<T> : PagingQueryConditionPrefetched<T>
{
    public ISort<T> Sorter { get; set; }

    public SortedPagingQueryConditionPrefetched(IFilter<T> f, IPrefetch p, ISort<T> s, int page, int pageSize) : base(f, p, page, pageSize)
    {
        Sorter = s;
    }
}

public class PagingQueryCondition<T> : QueryCondition<T>
{
    public int Page { get; set; }

    public int PageSize { get; set; }

    public PagingQueryCondition(int page, int pageSize)
    {
        Page = page;
        PageSize = pageSize;
    }

    public PagingQueryCondition(IFilter<T> f, int page, int pageSize) : base(f)
    {
        Page = page;
        PageSize = pageSize;
    }
}

public class SortedPagingQueryCondition<T> : PagingQueryCondition<T>
{
    public ISort<T> Sorter { get; set; }

    public SortedPagingQueryCondition(IFilter<T> f, ISort<T> s, int page, int pageSize) : base(f, page, pageSize)
    {
        Sorter = s;
    }
}

public static class QueryBuilder
{
    public static PagingSpec<T> Paging<T>(IQueryable<T> query, PagingQueryConditionPrefetched<T> qi, LinqMetaData? meta = null) where T : CommonEntityBase
    {
        if (qi.Prefetch == null)
            throw new ArgumentNullException($"{nameof(qi.Prefetch)} cannot be null");

        if (qi.Filter != null)
            query = qi.Filter.Filter(query, meta);

        var total = query;
        var result = query.WithPath(qi.Prefetch.Get()).Take(qi.PageSize).Skip((qi.Page - 1) * qi.PageSize);

        return new PagingSpec<T>
        {
            Count = total,
            Listing = result
        };
    }

    public static PagingSpec<T> Paging<T>(IQueryable<T> query, SortedPagingQueryConditionPrefetched<T> qi, LinqMetaData? meta = null) where T : CommonEntityBase
    {
        if (qi.Prefetch == null)
            throw new ArgumentNullException($"{nameof(qi.Prefetch)} cannot be null");

        if (qi.Sorter == null)
            throw new ArgumentNullException($"{nameof(qi.Sorter)} cannot be null");

        if (qi.Filter != null)
            query = qi.Filter.Filter(query, meta);

        var sorted = qi.Sorter.Sort(query);

        var total = sorted;
        var result = sorted.WithPath(qi.Prefetch.Get()).Take(qi.PageSize).Skip((qi.Page - 1) * qi.PageSize);

        return new PagingSpec<T>
        {
            Count = total,
            Listing = result
        };
    }

    public static PagingSpec<T> Paging<T>(IQueryable<T> query, PagingQueryCondition<T> qi, LinqMetaData? meta = null) where T : CommonEntityBase
    {
        if (qi.Filter != null)
            query = qi.Filter.Filter(query, meta);

        var total = query;
        var result = query.Take(qi.PageSize).Skip((qi.Page - 1) * qi.PageSize);

        return new PagingSpec<T>
        {
            Count = total,
            Listing = result
        };
    }

    public static PagingSpec<T> Paging<T>(IQueryable<T> query, SortedPagingQueryCondition<T> qi, LinqMetaData? meta = null) where T : CommonEntityBase
    {
        if (qi.Sorter == null)
            throw new ArgumentNullException($"{nameof(qi.Sorter)} cannot be null");

        if (qi.Filter != null)
            query = qi.Filter.Filter(query, meta);

        var sorted = qi.Sorter.Sort(query);

        var total = sorted;
        var result = sorted.Take(qi.PageSize).Skip((qi.Page - 1) * qi.PageSize);

        return new PagingSpec<T>
        {
            Count = total,
            Listing = result
        };
    }

    public static (PagingSpec<T>, IQueryable<T>) ReportPaging<T>(IQueryable<T> query, SortedPagingQueryCondition<T> qi, LinqMetaData? meta = null) where T : CommonEntityBase
    {
        if (qi.Sorter == null)
            throw new ArgumentNullException($"{nameof(qi.Sorter)} cannot be null");

        if (qi.Filter != null)
            query = qi.Filter.Filter(query, meta);

        var sorted = qi.Sorter.Sort(query);

        var total = sorted;
        var result = sorted.Take(qi.PageSize).Skip((qi.Page - 1) * qi.PageSize);

        return (new PagingSpec<T>
        {
            Count = total,
            Listing = result
        }, total);
    }

    public static (PagingSpec<T>, IQueryable<T>) ReportPagingForView<T>(IQueryable<T> query, SortedPagingQueryCondition<T> qi, LinqMetaData? meta = null) //where T : CommonEntityBase
    {
        if (qi.Sorter == null)
            throw new ArgumentNullException($"{nameof(qi.Sorter)} cannot be null");

        if (qi.Filter != null)
            query = qi.Filter.Filter(query, meta);

        var sorted = qi.Sorter.Sort(query);

        var total = sorted;
        var result = sorted.Take(qi.PageSize).Skip((qi.Page - 1) * qi.PageSize);

        return (new PagingSpec<T>
        {
            Count = total,
            Listing = result
        }, total);
    }

    public static ManySpec<T> Many<T>(IQueryable<T> query, QueryCondition<T> filter, LinqMetaData? meta = null) where T: CommonEntityBase
    {
        var result = filter.Filter!.Filter(query, meta);

        return new ManySpec<T>
        {
            Listing = result
        };
    }
}

public class SingleSpec<T>
{
    public IQueryable<T>? One { get; set; }
}

public class ManySpec<T>
{
    public IQueryable<T> Listing { get; set; } = default!;
}

public class PagingSpec<T> : ManySpec<T>
{
    public IQueryable<T> Count { get; set; } = default!;
}