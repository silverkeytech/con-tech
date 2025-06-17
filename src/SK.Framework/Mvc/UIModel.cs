using Microsoft.AspNetCore.Mvc.Rendering;

namespace SK.Framework.MVC;

public abstract class RootUI
{
    public Dictionary<string, List<SelectListItem>> DropDowns = new Dictionary<string, List<SelectListItem>>();

    public Dictionary<string, object> Properties = new Dictionary<string, object>();

    public object? GetProperty(string key) => Properties.ContainsKey(key) ? Properties[key] : null;

    public T? GetProperty<T>(string key) where T : class => GetProperty(key) as T;

    public void SetProperty(string key, object val) => Properties[key] = val;

    public bool PropertyExists(string key) => Properties.ContainsKey(key);
}

/// <summary>
/// Inherit from this class often for your view model
/// </summary>
/// <typeparam name="T"></typeparam>
public class ViewUI<T> : RootUI
{
    public T Item { get; set; } = default!;

    public ViewUI()
    {
    }

    public ViewUI(T item)
    {
        Item = item;
    }
}

public class EditUI<T> : RootUI
{
    public T Item { get; set; } = default!;

    public EditUI()
    {
    }

    public EditUI(T item)
    {
        Item = item;
    }
}

public class ListUI<T> : RootUI
{
    public List<T> Items { get; set; } = new();

    public ListUI()
    {
    }

    public ListUI(List<T> items)
    {
        Items = items;
    }

    public ListUI(IQuerySetMany<T> items)
    {
        Items = items.Items;
    }
}

public class ListUIWithInput<T, K> : ListUI<T>
{
    public K Input { get; set; } = default!;

    public ListUIWithInput()
    {
    }

    public ListUIWithInput(List<T> items)
    {
        Items = items;
    }
}

public class ItemUIWithInput<T, K> : RootUI
{
    public T Item { get; set; } = default!;

    public K Input { get; set; } = default!;

    public ItemUIWithInput()
    {
    }

    public ItemUIWithInput(T item)
    {
        Item = item;
    }
}

/// <summary>
/// Inherit from thsi class often for your paging view model
/// </summary>
/// <typeparam name="T"></typeparam>
public class ListUIWithPagingInfo<T> : ListUI<T>
{
    public PagingInfo PagingInfo { get; set; } = new();

    public ListUIWithPagingInfo()
    {
    }

    public ListUIWithPagingInfo(List<T> items, PagingInfo paging) : base(items)
    {
        PagingInfo = paging;
    }
}

public class ListUIWithComplexFilter<T, M> : ListUIWithPagingInfo<T>
{
    public M FilterUI { get; set; } = default!;

    public ListUIWithComplexFilter()
    {
    }

    public ListUIWithComplexFilter(List<T> items, PagingInfo paging, M filter) : base(items, paging)
    {
        FilterUI = filter;
    }
}
