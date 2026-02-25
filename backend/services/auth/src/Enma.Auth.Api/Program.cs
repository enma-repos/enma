using Enma.Auth.Application.Extensions;
using Enma.Auth.Infrastructure.ExternalAuth.Google.Extensions;
using Enma.Auth.Infrastructure.Grpc.Admin.Extensions;
using Enma.Auth.Infrastructure.Security.Extensions;
using Enma.Auth.Persistence.Postgres.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(o =>
{
    o.AddPolicy("OpenApiCors", p =>
        p.WithOrigins("http://localhost:8080") // где крутится Scalar (gateway)
            .AllowAnyHeader()
            .AllowAnyMethod()
    );
});

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi("v1");

builder.Services
    .AddApplication(builder.Configuration)
    .AddPersistence(builder.Configuration)
    .AddGoogleAuth(builder.Configuration)
    .AddSecurity(builder.Configuration)
    .AddAdminGrpcClient(builder.Configuration);

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