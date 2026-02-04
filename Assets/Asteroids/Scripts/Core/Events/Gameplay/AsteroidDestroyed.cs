using Asteroids.Core.ScriptableObjects.Data;
using ToolsACG.Core.EventBus;
using UnityEngine;
using Asteroids.Gameplay.Asteroids.Controllers;

namespace Asteroids.Core.Events.Gameplay
{
    public sealed class AsteroidDestroyed : IEvent
    {
        public AsteroidController AsteroidScript { get; }
        public SO_AsteroidData AsteroidData { get; }
        public Vector2 Position { get; }
        public Vector2 Direction { get; }

        public AsteroidDestroyed(AsteroidController asteroidScript, SO_AsteroidData asteroidData, Vector2 position, Vector2 direction)
        {
            AsteroidScript = asteroidScript;
            AsteroidData = asteroidData;
            Position = position;
            Direction = direction;
        }
    }
}