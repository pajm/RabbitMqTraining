using RabbitMqTraining.Application.Services;
using RabbitMqTraining.Core.Interfaces;
using RabbitMqTraining.Infrastructure.Messaging;
using RabbitMqTraining.Infrastructure.Models;

var options = new RabbitMqOptions();

var connectionFactory = new RabbitMqConnectionFactory(options);

await using var connection = await connectionFactory.CreateConnectionAsync();

var setup = new RabbitMqSetup(options);

await using var setupChannel = await connection.CreateChannelAsync();

await setup.InitialiseAsync(setupChannel);

var processor = new OrderProcessor();

var publisher = new RabbitMqPublisher(
    connection,
    options);

var consumer = new RabbitMqConsumer(
    connection,
    options,
    processor,
    publisher);

await consumer.StartAsync();

Console.WriteLine("Consumer running");

Console.ReadLine();