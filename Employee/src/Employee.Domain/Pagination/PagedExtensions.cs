namespace Employee.Domain.Pagination;

public static class PagedExtensions
{
    public static PagedResult<T> GetPagedNumbers<T>(
        this IEnumerable<T> values,
        int page,
        int pageSize) where T : class
    {
        var skip = (page - 1) * pageSize;
        var result = new PagedResult<T>
        {
            CurrentPage = page,
            PageSize = pageSize,
            RowCount = values.Count(),
            Skip = skip > 0 ? skip : 0
        };

        var pageCount = (double)result.RowCount / pageSize;
        result.PageCount = (int)Math.Ceiling(pageCount);
        if (result.PageCount < 0)
            result.PageCount = 0;

        return result;
    }
}