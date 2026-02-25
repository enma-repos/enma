using Enma.Admin.Application.Extensions;
using Enma.Admin.GrpcServer.Extensions;
using Enma.Admin.GrpcServer.Services;
using Enma.Admin.Persistence.Postgres.Extensions;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(o =>
{
    o.ConfigureEndpointDefaults(lo => lo.Protocols = HttpProtocols.Http1AndHttp2);
});

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.WithProperty("service", "enma-admin")
    .CreateLogger();

builder.Host.UseSerilog(Log.Logger, dispose: true);

builder.Services.AddCors(o =>
{
    o.AddPolicy("OpenApiCors", p =>
        p.WithOrigins("http://localhost:8080") // gateway
            .AllowAnyHeader()
            .AllowAnyMethod()
    );
});

builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddAdminGrpcServer();

builder.Services.AddControllers();
builder.Services.AddOpenApi("v1");

var app = builder.Build();

app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi().RequireCors("OpenApiCors");;
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapGrpcService<AdminUsersService>();
app.MapControllers();

app.Run();
