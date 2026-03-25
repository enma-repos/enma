using System.Text;
using Enma.Admin.Application.Extensions;
using Enma.Admin.GrpcServer.Extensions;
using Enma.Admin.GrpcServer.Services;
using Enma.Admin.Persistence.Postgres.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.IdentityModel.Tokens;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(o =>
{
    // REST (controllers) - HTTP/1.1 (+HTTP/2 if available)
    o.ListenAnyIP(8080, lo => lo.Protocols = HttpProtocols.Http1AndHttp2);
    // gRPC - HTTP/2 only
    o.ListenAnyIP(8081, lo => lo.Protocols = HttpProtocols.Http2);
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

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // options.Authority = builder.Configuration["Auth:Authority"];
        // options.RequireHttpsMetadata = true;

        var secretKey = builder.Configuration.GetSection("Jwt").GetValue<string>("SecretKey");
        if (string.IsNullOrWhiteSpace(secretKey))
        {
            throw new InvalidOperationException("JWT secret key is not configured (Jwt:SecretKey).");
        }

        options.TokenValidationParameters = new TokenValidationParameters
        {
            // We issue tokens locally (symmetric key) without Authority/metadata, so we must provide the signing key.
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddControllers();
builder.Services.AddOpenApi("v1");

var app = builder.Build();

app.Services.ApplyPendingMigrations();

app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi().RequireCors("OpenApiCors");;
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapGrpcService<AdminUsersService>();
app.MapControllers();

app.Run();
