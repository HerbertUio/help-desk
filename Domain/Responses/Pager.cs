namespace Domain.Responses;

public class Pager<T>
{
    public List<T> Items { get; private set; }
    public int TotalItems { get; private set; }
    public int PageIndex { get; private set; }
    public int PageSize { get; private set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);

    public Pager(List<T> items, int totalItems, int pageIndex, int pageSize)
    {
        Items = items ?? new List<T>();
        TotalItems = totalItems;
        PageIndex = pageIndex;
        PageSize = pageSize;
    }
}