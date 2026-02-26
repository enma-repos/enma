using Enma.Auth.Application.Extensions;
using Enma.Auth.Infrastructure.Caching.Extensions;
using Enma.Auth.Infrastructure.ExternalAuth.Google.Extensions;
using Enma.Auth.Infrastructure.Grpc.Admin.Extensions;
using Enma.Auth.Infrastructure.Security.Extensions;
using Enma.Auth.Persistence.Postgres.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.WithProperty("service", "enma-auth")
    .CreateLogger();

builder.Services.AddCors(o =>
{
    o.AddPolicy("OpenApiCors", p =>
        p.WithOrigins("http://localhost:8080") // где крутится Scalar (gateway)
            .AllowAnyHeader()
            .AllowAnyMethod()
    );
});

builder.Services.AddControllers();

builder.Services.AddOpenApi("v1");

builder.Services
    .AddApplication(builder.Configuration)
    .AddPersistence(builder.Configuration)
    .AddCachingService(builder.Configuration)
    .AddGoogleAuth(builder.Configuration)
    .AddSecurity(builder.Configuration)
    .AddAdminGrpcClient(builder.Configuration);

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // options.Authority = builder.Configuration["Auth:Authority"];
        // options.RequireHttpsMetadata = true;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
        };
    });

var app = builder.Build();

app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi().RequireCors("OpenApiCors");
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
