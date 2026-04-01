using FluentResults;

namespace Enma.Common.Models;

public sealed record PaginatedResult<T>
{
    public IReadOnlyList<T> Items { get; private init; } = [];
    public int TotalCount { get; private init; }
    public int Page { get; private init; }
    public int PageSize { get; private init; }

    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

    public bool HasPreviousPage => Page > 1;
    public bool HasNextPage => Page < TotalPages;

    private PaginatedResult() { }

    public static Result<PaginatedResult<T>> Create(IEnumerable<T> items, int totalCount, int page, int pageSize)
    {
        if (page < 1)
        {
            return Result.Fail<PaginatedResult<T>>("Page must be greater than 0.");
        }

        if (pageSize < 1)
        {
            return Result.Fail<PaginatedResult<T>>("Page size must be greater than 0.");
        }

        return Result.Ok(new PaginatedResult<T>
        {
            Items = items.ToList(),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        });
    }
}
