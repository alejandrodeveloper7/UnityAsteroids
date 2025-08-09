using System;
using System.Collections.Generic;

public class EventManager
{
    #region Fields

    //The more you separate the events in different bus more efficient and quick will be
    public static readonly EventManager GameplayBus = new EventManager();
    public static readonly EventManager UIBus = new EventManager();
    public static readonly EventManager InputBus = new EventManager();
    public static readonly EventManager SoundBus = new EventManager();
    public static readonly EventManager NetworkBus = new EventManager();

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

#region Event Examples

//ReadOnly struct is better for 1 or 2 primitives

//public readonly struct StopMusic : IEvent
//{
//    public readonly float ProgressivelyStopDuration;

//    public StopMusic(float progressivelyStopDuration)
//    {
//        ProgressivelyStopDuration = progressivelyStopDuration;
//    }
//}


//Sealed class is better for classes or 3 or more primitives

//public sealed class Generate2DSound : IEvent
//{
//    public List<SO_Sound> SoundsData { get; set; }

//    public Generate2DSound(List<SO_Sound> soundsData)
//    {
//        SoundsData = soundsData;
//    }
//}

#endregion

