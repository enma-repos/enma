using Enma.Auth.Application.Dto;
using FluentResults;

namespace Enma.Auth.Application.Contracts.Infrastructure.ExternalAuth;

public interface IExternalIdentityProvider
{
    string Name { get; }
    Task<Result<ExternalIdentityDto>> AuthenticateAsync(ExternalAuthRequestDto request, CancellationToken ct);
}