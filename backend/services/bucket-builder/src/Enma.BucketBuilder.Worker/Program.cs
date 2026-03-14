using Enma.BucketBuilder.Application.Extensions;
using Enma.BucketBuilder.JobsOrchestration.Extensions;
using Enma.BucketBuilder.Persistence.ClickHouse.Extensions;
using Enma.BucketBuilder.Persistence.Mongo.Extensions;
using Enma.BucketBuilder.Worker.Jobs;
using Quartz;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.WithProperty("service", "enma-bucket-builder")
    .CreateLogger();

builder.Services.AddSerilog(Log.Logger, dispose: true);

builder.Services
    .AddApplication()
    .AddJobsOrchestration(builder.Configuration)
    .AddClickHousePersistence(builder.Configuration)
    .AddMongoPersistence(builder.Configuration);

var bucketBuilderConfig = builder.Configuration.GetSection("BucketBuilder");
var shardCount = bucketBuilderConfig.GetValue("ShardCount", 1);
var pollInterval = bucketBuilderConfig.GetValue("PollIntervalSeconds", 10);

builder.Services.AddQuartz(q =>
{
    for (var shardIndex = 0; shardIndex < shardCount; shardIndex++)
    {
        var jobKey = new JobKey($"bucket-build-shard-{shardIndex}");

        q.AddJob<BucketBuildTickJob>(opts => opts
            .WithIdentity(jobKey)
            .UsingJobData("ShardIndex", shardIndex)
            .UsingJobData("ShardCount", shardCount));

        q.AddTrigger(opts => opts
            .ForJob(jobKey)
            .WithIdentity($"bucket-build-shard-{shardIndex}-trigger")
            .WithSimpleSchedule(x => x
                .WithIntervalInSeconds(pollInterval)
                .RepeatForever())
            .StartNow());
    }
});

builder.Services.AddQuartzHostedService(q =>
{
    q.WaitForJobsToComplete = true;
});

await builder.Build().RunAsync();
