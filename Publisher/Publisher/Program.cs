using System;
using MassTransit;
using Messages;

namespace Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            var bus = Bus.Factory.CreateUsingRabbitMq(configurator =>
            {
                configurator.Host(new Uri("rabbitmq://192.168.56.101"), hostConfigurator =>
                {
                    hostConfigurator.Username("test");
                    hostConfigurator.Password("test");

                    hostConfigurator.PublisherConfirmation = true;
                });
            });

            bus.Start();
            bus.Publish<IHelloWorldMessage>(new { Message = "Hello, World!" });
            bus.Stop();
        }
    }
}
