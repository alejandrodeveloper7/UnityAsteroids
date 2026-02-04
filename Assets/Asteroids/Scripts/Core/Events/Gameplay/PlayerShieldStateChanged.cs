using ACG.Core.EventBus;

namespace Asteroids.Core.Events.Gameplay
{
    public readonly struct PlayerShieldStateChanged : IEvent
    {
        public readonly bool Active;

        public PlayerShieldStateChanged(bool active)
        {
            Active = active;
        }
    }
}