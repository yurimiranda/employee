using Employee.Domain.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Employee.Infra.EFCore.Extensions;

internal static class Pagination
{
    internal static async Task<PagedResult<T>> GetPaged<T>(
        this IQueryable<T> query,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default) where T : class
    {
        var result = query.GetPagedNumbers(page, pageSize);

        if (result.Skip > 0)
            query = query.Skip(result.Skip);

        if (pageSize > 0)
            query = query.Take(pageSize);

        result.Results = await query.ToListAsync(cancellationToken);
        return result;
    }
}