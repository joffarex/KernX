using System;
using System.Threading.Tasks;

namespace KernX.EventBus
{
    public interface IEventBus
    {
        Task Publish<T>(string topic, string appId, T @event) where T : IEvent;
        Task Subscribe<T>(string queue, Func<EventHeaders, T, Task> callback);
    }
}