using System;
using System.Threading.Tasks;

namespace KernX.EventBus
{
    public interface IEventBus
    {
        void Publish<T>(string exchange, string appId, T @event) where T : IEvent;
        void Subscribe<T>(string queueName, Func<EventHeaders, T, Task> callback);
    }
}