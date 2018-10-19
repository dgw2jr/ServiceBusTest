using System.Threading;
using System.Threading.Tasks;

namespace ServiceBus
{
    public interface IServiceBus
    {
        Task Publish<T>(T message, CancellationToken cancellationToken = default(CancellationToken)) where T : class;
        Task Publish<T>(object message, CancellationToken cancellationToken = default(CancellationToken));
    }
}
