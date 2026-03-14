using Enma.Analytics.Application.Extensions;
using Enma.Analytics.Persistence.Mongo.Extensions;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.WithProperty("service", "enma-analytics")
    .CreateLogger();

builder.Host.UseSerilog(Log.Logger, dispose: true);

builder.Services.AddCors(o =>
{
    o.AddPolicy("OpenApiCors", p =>
        p.WithOrigins("http://localhost:8080")
            .AllowAnyHeader()
            .AllowAnyMethod()
    );
});

builder.Services.AddApplication();
builder.Services.AddMongoPersistence(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddOpenApi("v1");

builder.WebHost.ConfigureKestrel(o =>
{
    o.ListenAnyIP(8080, lo => lo.Protocols = HttpProtocols.Http1AndHttp2);
});

var app = builder.Build();

app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi().RequireCors("OpenApiCors");
}

app.UseAuthorization();

app.MapControllers();

app.Run();
