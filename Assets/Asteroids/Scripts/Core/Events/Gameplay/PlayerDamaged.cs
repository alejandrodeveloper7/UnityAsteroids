using ToolsACG.Core.EventBus;

namespace Asteroids.Core.Events.Gameplay
{
    public readonly struct PlayerDamaged : IEvent
    {
        public readonly int Health;

        public PlayerDamaged(int health)
        {
            Health = health;
        }
    }
}