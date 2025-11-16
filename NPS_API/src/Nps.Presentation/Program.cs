using Microsoft.OpenApi.Models;
using FluentValidation;
using MediatR;
using Nps.Application;
var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "NPS API",
        Version = "v1"
    });
});

builder.Services.AddAutoMapper(typeof(AssemblyReference).Assembly);

builder.Services.AddMediatR(typeof(AssemblyReference).Assembly);

builder.Services.AddValidatorsFromAssembly(typeof(AssemblyReference).Assembly);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "NPS API v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
