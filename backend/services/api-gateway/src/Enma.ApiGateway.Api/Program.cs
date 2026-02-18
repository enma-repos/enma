using System.Net;
using System.Threading.RateLimiting;
using Enma.ApiGateway.Api.Constants;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

#region Configuration

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    // .AddJsonFile(builder.Configuration.GetValue<string>("ReverseProxyConfigFilePath") 
    //             ?? throw new InvalidOperationException(), optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

#endregion

#region Logger

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.WithProperty("service", "enma-api-gateway")
    .CreateLogger();

builder.Host.UseSerilog(Log.Logger, dispose: true);

#endregion

#region Cors

var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];

builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultCors", p =>
    {
        p.WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

#endregion

#region Rate limiting

builder.Services.AddRateLimiter(o =>
{
    o.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    o.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(ctx =>
    {
        var ip = ctx.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        return RateLimitPartition.GetFixedWindowLimiter(ip, _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = ApiGatewayConstants.PermitLimit,
            Window = TimeSpan.FromMinutes(ApiGatewayConstants.WindowFromMinutes),
            QueueLimit = ApiGatewayConstants.QueueLimit,
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst
        });
    });
});

#endregion

#region Auth

// TODO: add auth

#endregion

#region Reverse Proxy

builder.Services
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .ConfigureHttpClient((sp, handler) =>
    {
        handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
        handler.PooledConnectionLifetime = TimeSpan.FromMinutes(ApiGatewayConstants.PooledConnectionLifetimeFromMinutes);
        handler.PooledConnectionIdleTimeout = TimeSpan.FromMinutes(ApiGatewayConstants.PooledConnectionIdleTimeoutFromMinutes);
        handler.MaxConnectionsPerServer = ApiGatewayConstants.MaxConnectionsPerServer;
        handler.ConnectTimeout = TimeSpan.FromSeconds(ApiGatewayConstants.ConnectTimeoutFromSeconds);
    });

#endregion

#region Kestrel limits

builder.WebHost.ConfigureKestrel(k =>
{
    k.Limits.MaxRequestBodySize = ApiGatewayConstants.MaxRequestBodySize;
    k.Limits.RequestHeadersTimeout = TimeSpan.FromSeconds(ApiGatewayConstants.RequestHeadersTimeoutFromSeconds);
    k.Limits.KeepAliveTimeout = TimeSpan.FromSeconds(ApiGatewayConstants.KeepAliveTimeoutFromSeconds);
});

#endregion

builder.Services.AddOpenApi(ApiGatewayConstants.ApiVersion);
builder.Services.AddHealthChecks();

var app = builder.Build();
var isEnvDevelopment = app.Environment.IsDevelopment();

app.UseSerilogRequestLogging();

app.Use(async (ctx, next) =>
{
    ctx.Response.Headers.TryAdd("X-Content-Type-Options", "nosniff");
    ctx.Response.Headers.TryAdd("X-Frame-Options", "DENY");
    ctx.Response.Headers.TryAdd("Referrer-Policy", "no-referrer");
    
    if (!isEnvDevelopment)
    {
        ctx.Response.Headers.TryAdd("Strict-Transport-Security", "max-age=15552000; includeSubDomains; preload");
    }

    await next();
});

if (!isEnvDevelopment)
{
    app.UseHsts();
}
app.UseHttpsRedirection();

app.UseRateLimiter();
app.UseCors("DefaultCors");

// app.UseAuthentication();
app.UseAuthorization();

if (isEnvDevelopment)
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Enma API Gateway");
        
        options
            .AddDocument("auth", "Auth API", "http://localhost:8082/openapi/v1.json", isDefault: true)
            .AddDocument("project", "Project API", "http://localhost:8084/openapi/v1.json")
            .AddDocument("analytics", "Analytics API", "http://localhost:8081/openapi/v1.json");
    });
}

app.MapHealthChecks("/health").AllowAnonymous();

app.MapReverseProxy(proxyPipeline =>
{
    proxyPipeline.Use(async (ctx, next) =>
    {
        if (ctx.Request.Path.Value is { Length: > ApiGatewayConstants.MaxUriLength })
        {
            ctx.Response.StatusCode = StatusCodes.Status414UriTooLong;
            return;
        }

        await next();
    });
});

app.Run();