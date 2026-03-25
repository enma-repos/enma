using Enma.Admin.Application.Contracts;
using Enma.Admin.Application.Models;
using Enma.Admin.Persistence.Postgres.Connection;
using Enma.Admin.Persistence.Postgres.Entities;
using Enma.Admin.Persistence.Postgres.Mappers;
using Enma.Common.Errors;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Enma.Admin.Persistence.Postgres.Repositories;

internal sealed class NotificationsRepository : INotificationsRepository
{
    private readonly PostgresDbContext _context;

    public NotificationsRepository(PostgresDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Notification>> CreateAsync(Notification notification, CancellationToken ct = default)
    {
        _context.Notifications.Add(notification.ToEntity());
        await _context.SaveChangesAsync(ct);
        return Result.Ok(notification);
    }

    public async Task<Result<IReadOnlyList<Notification>>> ListByRecipientAsync(
        Guid recipientUserId, bool unreadOnly, int offset, int limit, CancellationToken ct = default)
    {
        var query = _context.Notifications
            .AsNoTracking()
            .Where(x => x.RecipientUserId == recipientUserId);

        if (unreadOnly)
        {
            query = query.Where(x => !x.IsRead);
        }

        var entities = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip(offset)
            .Take(limit)
            .Select(x => new NotificationEntity
            {
                Id = x.Id,
                RecipientUserId = x.RecipientUserId,
                Type = x.Type,
                Title = x.Title,
                Message = x.Message,
                ResourceId = x.ResourceId,
                IsRead = x.IsRead,
                CreatedAt = x.CreatedAt,
                ReadAt = x.ReadAt
            })
            .ToListAsync(ct);

        return Result.Ok<IReadOnlyList<Notification>>(entities.Select(x => x.ToModel()).ToList());
    }

    public async Task<Result<int>> GetUnreadCountAsync(Guid recipientUserId, CancellationToken ct = default)
    {
        var count = await _context.Notifications
            .AsNoTracking()
            .CountAsync(x => x.RecipientUserId == recipientUserId && !x.IsRead, ct);

        return Result.Ok(count);
    }

    public async Task<Result> SetReadAsync(Guid notificationId, Guid recipientUserId, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var affected = await _context.Notifications
            .Where(x => x.Id == notificationId && x.RecipientUserId == recipientUserId && !x.IsRead)
            .ExecuteUpdateAsync(
                s => s
                    .SetProperty(x => x.IsRead, true)
                    .SetProperty(x => x.ReadAt, now),
                ct);

        return affected == 0
            ? Result.Fail(ApplicationErrors.EntityNotFound("Notification", $"id={notificationId}"))
            : Result.Ok();
    }

    public async Task<Result> SetAllReadAsync(Guid recipientUserId, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        await _context.Notifications
            .Where(x => x.RecipientUserId == recipientUserId && !x.IsRead)
            .ExecuteUpdateAsync(
                s => s
                    .SetProperty(x => x.IsRead, true)
                    .SetProperty(x => x.ReadAt, now),
                ct);

        return Result.Ok();
    }

    public async Task<Result> DeleteAsync(Guid notificationId, Guid recipientUserId, CancellationToken ct = default)
    {
        var affected = await _context.Notifications
            .Where(x => x.Id == notificationId && x.RecipientUserId == recipientUserId)
            .ExecuteDeleteAsync(ct);

        return affected == 0
            ? Result.Fail(ApplicationErrors.EntityNotFound("Notification", $"id={notificationId}"))
            : Result.Ok();
    }
}
