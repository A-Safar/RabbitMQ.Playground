using Services.Consumers;
using Services.Contracts;
using Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IRabbitMQService, RabbitMQService>();

// Listen for messages
//builder.Services.AddHostedService<DirectQueue1Consumer>();

var app = builder.Build();

// Resolve IRabbitMQService to create the RabbitMQService instance and the queues
var rabbitMqService = app.Services.GetRequiredService<IRabbitMQService>();

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
