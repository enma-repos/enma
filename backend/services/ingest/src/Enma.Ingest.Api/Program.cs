using Enma.Ingest.Application.Extensions;
using Enma.Ingest.Infrastructure.RabbitMq.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.WithProperty("service", "enma-ingest")
    .CreateLogger();

builder.Host.UseSerilog(Log.Logger, dispose: true);

builder.Services.AddCors(o =>
{
    o.AddPolicy("OpenApiCors", p =>
        p.WithOrigins("http://localhost:8080") // где крутится Scalar (gateway)
            .AllowAnyHeader()
            .AllowAnyMethod()
    );
});

builder.Services
    .AddApplication(builder.Configuration)
    .AddRabbitMqMessaging(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddOpenApi("v1");

var app = builder.Build();

app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi().RequireCors("OpenApiCors");;
}
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();