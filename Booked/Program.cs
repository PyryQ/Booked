using Booked;
using Booked.Models.Classes;
using Microsoft.EntityFrameworkCore;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var environment = builder.Environment.EnvironmentName.ToLower();
builder.Configuration.AddSystemsManager($"/{environment}/booked", new AWSOptions {Credentials= AWSCredentials.Validate() });

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<ExampleSettings>(builder.Configuration.GetSection("ExampleSettings"));

builder.Services.AddDbContext<BookDataContext>(
    o => o.UseNpgsql(builder.Configuration.GetConnectionString("BookedPostgresAlpine"))
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
