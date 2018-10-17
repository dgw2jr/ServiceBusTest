using System;
using System.Threading.Tasks;
using MassTransit;
using Messages;
using Console = System.Console;

namespace Subscriber
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
                    });

                    configurator.ReceiveEndpoint("hello_world", endpointConfigurator =>
                    {
                        endpointConfigurator.Consumer<HelloWorldConsumer>();
                    });
                });

            bus.Start();

            Console.ReadLine();

            bus.Stop();
        }
    }

    public class HelloWorldConsumer :IConsumer<IHelloWorldMessage>
    {
        public Task Consume(ConsumeContext<IHelloWorldMessage> context)
        {
            Console.WriteLine(context.Message.Message);

            return Task.CompletedTask;
        }
    }
}
