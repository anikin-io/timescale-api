using Microsoft.EntityFrameworkCore;
using TimescaleApi.API.Extensions;
using TimescaleApi.API.Middleware;
using TimescaleApi.Application.Services;
using TimescaleApi.Infrastructure.Persistence;
using TimescaleApi.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// DI container
builder.Services.AddDbContext<TimescaleDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IUploadService, UploadService>();
builder.Services.AddScoped<IResultQueryService, ResultQueryService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.ApplyMigrations();

// Middleware pipeline
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
