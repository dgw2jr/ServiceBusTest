using System;
using System.Threading.Tasks;
using Autofac;
using MassTransit;
using Messages;
using Console = System.Console;

namespace Subscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var scope = BuildContainer().BeginLifetimeScope())
            {
                var bus = scope.Resolve<IBusControl>();

                bus.Start();

                Console.ReadLine();

                bus.Stop();
            }
        }

        static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<HelloWorldConsumer>().AsImplementedInterfaces();
            builder.Register(c =>
            {
                var bus = Bus.Factory.CreateUsingRabbitMq(configurator =>
                {
                    configurator.Host(new Uri("rabbitmq://192.168.56.101"), hostConfigurator =>
                    {
                        hostConfigurator.Username("test");
                        hostConfigurator.Password("test");
                    });

                    configurator.ReceiveEndpoint("hello_world",
                        endpointConfigurator =>
                        {
                            // Load consumers from the container
                            endpointConfigurator.LoadFrom(c);
                        });
                });

                return bus;
            })
                .As<IBus>()
                .As<IBusControl>()
                .SingleInstance();

            return builder.Build();
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
