namespace TodoApp.Application.Common.Models;

public class PagedResult<T>
{
    public IReadOnlyList<T> Items { get; }
    public int TotalCount { get; }
    public bool HasNextPage { get; }

    public PagedResult(IReadOnlyList<T> items, int totalCount, bool hasNextPage)
    {
        Items = items;
        TotalCount = totalCount;
        HasNextPage = hasNextPage;
    }
}
