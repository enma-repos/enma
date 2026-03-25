namespace Enma.Admin.Application.Dto;

public sealed record PagedResult<T>(IReadOnlyList<T> Items, int Total);
