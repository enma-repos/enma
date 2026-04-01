namespace Enma.EventProcessor.Infrastructure.Grpc.Admin.Options;

public sealed record AdminGrpcOptions
{
    public required string Address { get; init; }
    public int DeadlineMs { get; init; } = 3000;
}
