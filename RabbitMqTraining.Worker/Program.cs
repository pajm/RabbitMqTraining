using RabbitMqTraining.Application.Services;
using RabbitMqTraining.Core.Interfaces;
using RabbitMqTraining.Infrastructure.Messaging;
using RabbitMqTraining.Infrastructure.Models;
using RabbitMqTraining.Worker;

var builder = Host.CreateApplicationBuilder(args);


var options = new RabbitMqOptions();


var connectionFactory = new RabbitMqConnectionFactory(options);

var connection = await connectionFactory.CreateConnectionAsync();


builder.Services.AddSingleton(options);

builder.Services.AddSingleton(connection);

builder.Services.AddSingleton<RabbitMqConsumer>();

builder.Services.AddSingleton<RabbitMqSetup>();

builder.Services.AddSingleton<IOrderProcessor, OrderProcessor>();

builder.Services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();

builder.Services.AddHostedService<OrderWorker>();


var host = builder.Build();

await host.RunAsync();