using Enma.Admin.Application.Abstractions;
using Enma.Admin.Application.Dto.Users;
using Enma.Api.Shared.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Enma.Admin.Api.Controllers.v1;

[Route("api/admin/v1/users")]
[ApiController]
[Authorize]
public sealed class UsersController(IUsersService usersService) : ControllerBase
{
    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> GetByIdAsync(
        [FromRoute] Guid userId,
        CancellationToken ct)
    {
        var res = await usersService.GetByIdAsync(userId, ct);
        return res.ToActionResult();
    }

    [HttpGet("{userId:guid}/exists")]
    public async Task<IActionResult> ExistsAsync(
        [FromRoute] Guid userId,
        CancellationToken ct)
    {
        var res = await usersService.ExistsAsync(userId, ct);
        return res.ToActionResult();
    }

    [HttpPatch("{userId:guid}/display-name")]
    public async Task<IActionResult> SetDisplayNameAsync(
        [FromRoute] Guid userId,
        [FromBody] SetUserDisplayNameDto dto,
        CancellationToken ct)
    {
        if (!VerifyOwnership(userId)) return Forbid();
        var res = await usersService.SetDisplayNameAsync(userId, dto, ct);
        return res.ToActionResult();
    }

    [HttpPatch("{userId:guid}/avatar-url")]
    public async Task<IActionResult> SetAvatarUrlAsync(
        [FromRoute] Guid userId,
        [FromBody] SetUserAvatarUrlDto dto,
        CancellationToken ct)
    {
        if (!VerifyOwnership(userId)) return Forbid();
        var res = await usersService.SetAvatarUrlAsync(userId, dto, ct);
        return res.ToActionResult();
    }

    [HttpPatch("{userId:guid}/locale")]
    public async Task<IActionResult> SetLocaleAsync(
        [FromRoute] Guid userId,
        [FromBody] SetUserLocaleDto dto,
        CancellationToken ct)
    {
        if (!VerifyOwnership(userId)) return Forbid();
        var res = await usersService.SetLocaleAsync(userId, dto, ct);
        return res.ToActionResult();
    }

    [HttpPatch("{userId:guid}/timezone")]
    public async Task<IActionResult> SetTimezoneAsync(
        [FromRoute] Guid userId,
        [FromBody] SetUserTimezoneDto dto,
        CancellationToken ct)
    {
        if (!VerifyOwnership(userId)) return Forbid();
        var res = await usersService.SetTimezoneAsync(userId, dto, ct);
        return res.ToActionResult();
    }

    [HttpDelete("{userId:guid}")]
    public async Task<IActionResult> SoftDeleteAsync(
        [FromRoute] Guid userId,
        CancellationToken ct)
    {
        if (!VerifyOwnership(userId)) return Forbid();
        var res = await usersService.SoftDeleteAsync(userId, ct);
        return res.ToActionResult();
    }

    private bool VerifyOwnership(Guid userId)
        => User.TryGetAccountId(out var accountId) && accountId == userId;
}
