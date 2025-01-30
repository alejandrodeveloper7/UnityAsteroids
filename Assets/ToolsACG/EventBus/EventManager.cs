using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ToolsACG.Utils.Events
{
    public interface IEvent
    {
    }

    public class EventManager
    {
        static readonly EventManager busUI = new EventManager();
        static readonly EventManager busNetwork = new EventManager();
        static readonly EventManager busGameplay = new EventManager();

        public delegate void EventDelegate<T>(T pEvent) where T : IEvent;
        private delegate void EventDelegate(IEvent pEvent);

        private readonly Dictionary<Type, EventDelegate> delegates = new Dictionary<Type, EventDelegate>();
        private readonly Dictionary<Delegate, EventDelegate> delegateLookup = new Dictionary<Delegate, EventDelegate>();

        public static EventManager GetUiBus()
        {
            return busUI;
        }

        public static EventManager GetNetworkBus()
        {
            return busNetwork;
        }

        public static EventManager GetGameplayBus()
        {
            return busGameplay;
        }

        public void AddListener<T>(EventDelegate<T> pDelegate) where T : IEvent
        {
            if (delegateLookup.ContainsKey(pDelegate))
                return;

            EventDelegate internalDelegate = e => pDelegate((T)e);
            delegateLookup[pDelegate] = internalDelegate;

            EventDelegate temporalDelegate;
            if (delegates.TryGetValue(typeof(T), out temporalDelegate))
                delegates[typeof(T)] = temporalDelegate += internalDelegate;
            else
                delegates[typeof(T)] = internalDelegate;
        }

        public void RemoveListener<T>(EventDelegate<T> pDelegate) where T : IEvent
        {
            EventDelegate internalDelegate;
            if (delegateLookup.TryGetValue(pDelegate, out internalDelegate))
            {
                EventDelegate temporalDelegate;
                if (delegates.TryGetValue(typeof(T), out temporalDelegate))
                {
                    temporalDelegate -= internalDelegate;
                    if (temporalDelegate == null)
                        delegates.Remove(typeof(T));
                    else
                        delegates[typeof(T)] = temporalDelegate;
                }
                delegateLookup.Remove(pDelegate);
            }
        }

        public Task RaiseEvent(IEvent pEvent)
        {
            EventDelegate temporalDelegate;
            if (delegates.TryGetValue(pEvent.GetType(), out temporalDelegate))
            {
                temporalDelegate.Invoke(pEvent);
            }

            return Task.CompletedTask;
        }
    }
}
