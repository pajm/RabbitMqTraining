using RabbitMQ.Client;
using RabbitMqTraining.Core.Interfaces;
using RabbitMqTraining.Infrastructure.Messaging;
using RabbitMqTraining.Infrastructure.Models;

var builder = WebApplication.CreateBuilder(args);


var options = new RabbitMqOptions();

var connectionFactory = new RabbitMqConnectionFactory(options);

var connection = await connectionFactory.CreateConnectionAsync();


builder.Services.AddSingleton(options);

builder.Services.AddSingleton<IConnection>(connection);

builder.Services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();


builder.Services.AddControllers();


var app = builder.Build();

app.MapControllers();

app.Run();