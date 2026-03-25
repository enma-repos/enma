using Enma.Admin.Application.Abstractions;
using Enma.Api.Shared.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Enma.Admin.Api.Controllers.v1;

[Route("api/admin/v1/notifications")]
[ApiController]
[Authorize]
public sealed class NotificationsController(INotificationsService notificationsService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> ListAsync(
        [FromQuery] bool unreadOnly = false,
        [FromQuery] int offset = 0,
        [FromQuery] int limit = 20,
        CancellationToken ct = default)
    {
        if (!TryGetAccountId(out var accountId))
            return Unauthorized();

        var res = await notificationsService.ListByRecipientAsync(accountId, unreadOnly, offset, limit, ct);
        return res.ToActionResult();
    }

    [HttpGet("unread-count")]
    public async Task<IActionResult> GetUnreadCountAsync(CancellationToken ct = default)
    {
        if (!TryGetAccountId(out var accountId))
            return Unauthorized();

        var res = await notificationsService.GetUnreadCountAsync(accountId, ct);
        return res.ToActionResult();
    }

    [HttpPatch("{notificationId:guid}/read")]
    public async Task<IActionResult> SetReadAsync(
        [FromRoute] Guid notificationId,
        CancellationToken ct)
    {
        if (!TryGetAccountId(out var accountId))
            return Unauthorized();

        var res = await notificationsService.SetReadAsync(notificationId, accountId, ct);
        return res.ToActionResult();
    }

    [HttpPatch("read-all")]
    public async Task<IActionResult> SetAllReadAsync(CancellationToken ct)
    {
        if (!TryGetAccountId(out var accountId))
            return Unauthorized();

        var res = await notificationsService.SetAllReadAsync(accountId, ct);
        return res.ToActionResult();
    }

    [HttpDelete("{notificationId:guid}")]
    public async Task<IActionResult> DeleteAsync(
        [FromRoute] Guid notificationId,
        CancellationToken ct)
    {
        if (!TryGetAccountId(out var accountId))
            return Unauthorized();

        var res = await notificationsService.DeleteAsync(notificationId, accountId, ct);
        return res.ToActionResult();
    }

    private bool TryGetAccountId(out Guid accountId)
        => User.TryGetAccountId(out accountId);
}
