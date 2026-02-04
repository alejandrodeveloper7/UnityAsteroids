using System;
using System.Collections.Generic;
using UnityEngine;

namespace ACG.Core.EventBus
{
    public class EventBusManager
    {
        #region Fields

        [Header("Bus")]
        public static readonly EventBusManager GameFlowBus = new();
        public static readonly EventBusManager GameplayBus = new();

        [Header("Values")]
        private readonly Dictionary<Type, Delegate> _events = new();

        #endregion

        #region Events Management

        public void AddListener<T>(Action<T> calback) where T : IEvent
        {
            Type type = typeof(T);

            if (!_events.ContainsKey(type))
                _events[type] = null;

            _events[type] = (Action<T>)_events[type] + calback;
        }
        public void RemoveListener<T>(Action<T> callback) where T : IEvent
        {
            Type type = typeof(T);
            if (_events.TryGetValue(type, out var existingDelegate))
            {
                _events[type] = (Action<T>)existingDelegate - callback;
                if (_events[type] == null) _events.Remove(type);
            }
        }
        public void RaiseEvent<T>(T data) where T : IEvent
        {
            Type type = typeof(T);

            if (_events.TryGetValue(type, out var existingDelegate) && existingDelegate is Action<T> action)
                action.Invoke(data);
        }

        #endregion
    }
}
