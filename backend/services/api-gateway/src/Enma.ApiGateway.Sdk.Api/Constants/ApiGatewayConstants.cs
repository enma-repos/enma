namespace Enma.ApiGateway.Sdk.Api.Constants;

internal static class ApiGatewayConstants
{
    // documentation
    public const string ApiVersion = "v1";
    
    // rate limiting
    public const int PermitLimit = 200;
    public const int WindowFromMinutes = 1;
    public const int QueueLimit = 0;
    
    // reverse proxy
    public const int PooledConnectionLifetimeFromMinutes = 5;
    public const int PooledConnectionIdleTimeoutFromMinutes = 2;
    public const int MaxConnectionsPerServer = 256;
    public const int ConnectTimeoutFromSeconds = 5;
    public const int MaxUriLength = 2048;
    
    // kestrel limits
    public const int MaxRequestBodySize = 10 * 1024 * 1024; // 10 MB
    public const int RequestHeadersTimeoutFromSeconds = 10;
    public const int KeepAliveTimeoutFromSeconds = 120;
}