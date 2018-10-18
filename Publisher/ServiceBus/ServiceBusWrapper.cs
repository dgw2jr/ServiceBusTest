using System.Threading;
using System.Threading.Tasks;
using MassTransit;

namespace ServiceBus
{
    public class ServiceBusWrapper : IServiceBus
    {
        private readonly IBus _bus;

        public ServiceBusWrapper(IBus bus)
        {
            _bus = bus;
        }

        public Task Publish<T>(T message, CancellationToken cancellationToken = default(CancellationToken)) where T : class 
        {
            return _bus.Publish(message, cancellationToken);
        }

        public Task Publish<T>(object message, CancellationToken cancellationToken = default(CancellationToken))
        {
            return _bus.Publish(message, cancellationToken);
        }
    }
}