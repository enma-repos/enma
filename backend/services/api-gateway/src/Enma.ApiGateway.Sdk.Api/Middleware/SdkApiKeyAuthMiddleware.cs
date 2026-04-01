using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Enma.ApiGateway.Sdk.Infrastructure.Caching.Abstractions;
using Enma.ApiGateway.Sdk.Infrastructure.Grpc.Admin.Abstractions;

namespace Enma.ApiGateway.Sdk.Api.Middleware;

public sealed class SdkApiKeyAuthMiddleware
{
    private const string ApiKeyHeader = "X-Api-Key";

    private readonly RequestDelegate _next;
    private readonly ILogger<SdkApiKeyAuthMiddleware> _logger;

    public SdkApiKeyAuthMiddleware(RequestDelegate next, ILogger<SdkApiKeyAuthMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(
        HttpContext context,
        IAdminApiKeysClient adminClient,
        ISdkAuthCacheService cacheService)
    {
        if (context.Request.Path.StartsWithSegments("/health"))
        {
            await _next(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue(ApiKeyHeader, out var apiKeyValues)
            || string.IsNullOrWhiteSpace(apiKeyValues.FirstOrDefault()))
        {
            await WriteUnauthorized(context, "API key is required.");
            return;
        }

        var plainKey = apiKeyValues.First()!;
        var cacheKey = ComputeCacheKey(plainKey);

        var cached = await cacheService.GetAsync(cacheKey);
        if (cached is not null)
        {
            SetContextHeaders(context, cached.OrganizationId, cached.ProjectId, cached.SdkClientId);
            await _next(context);
            return;
        }

        var result = await adminClient.ValidateAsync(plainKey, context.RequestAborted);
        if (result.IsFailed)
        {
            _logger.LogWarning("SDK API key validation failed: {Error}", result.Errors.FirstOrDefault()?.Message);
            await WriteUnauthorized(context, "Invalid API key.");
            return;
        }

        var validation = result.Value;
        await cacheService.SetAsync(cacheKey, validation);

        SetContextHeaders(context, validation.OrganizationId, validation.ProjectId, validation.SdkClientId);
        await _next(context);
    }

    private static void SetContextHeaders(HttpContext context, Guid orgId, Guid projectId, Guid sdkClientId)
    {
        context.Request.Headers["X-Organization-Id"] = orgId.ToString();
        context.Request.Headers["X-Project-Id"] = projectId.ToString();
        context.Request.Headers["X-Sdk-Client-Id"] = sdkClientId.ToString();
    }

    private static string ComputeCacheKey(string plainKey)
    {
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(plainKey));
        return Convert.ToHexString(hash);
    }

    private static async Task WriteUnauthorized(HttpContext context, string message)
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(
            JsonSerializer.Serialize(new { error = message }));
    }
}
