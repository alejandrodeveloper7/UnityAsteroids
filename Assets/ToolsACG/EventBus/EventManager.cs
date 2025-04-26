using System;
using System.Collections.Generic;

namespace ToolsACG.Utils.Events
{
    public class EventManager
    {
        #region Fields

        public static readonly EventManager UIBus = new EventManager();
        public static readonly EventManager NetworkBus = new EventManager();
        public static readonly EventManager GameplayBus = new EventManager();
        public static readonly EventManager InputBus = new EventManager();
        public static readonly EventManager SoundBus = new EventManager();

        private Dictionary<Type, Delegate> _events = new();

        #endregion

        #region EventsManagement

        public void AddListener<T>(Action<T> pCalback) where T : IEvent
        {
            Type type = typeof(T);

            if (!_events.ContainsKey(type))
                _events[type] = null;

            _events[type] = (Action<T>)_events[type] + pCalback;
        }

        public void RemoveListener<T>(Action<T> pCallback) where T : IEvent
        {
            Type type = typeof(T);
            if (_events.TryGetValue(type, out var existingDelegate))
            {
                _events[type] = (Action<T>)existingDelegate - pCallback;
                if (_events[type] == null) _events.Remove(type);
            }
        }

        public void RaiseEvent<T>(T pData) where T : IEvent
        {
            Type type = typeof(T);

            if (_events.TryGetValue(type, out var existingDelegate) && existingDelegate is Action<T> action)
                action.Invoke(pData);
        }

        #endregion
    }

    public interface IEvent
    {
    }
}
