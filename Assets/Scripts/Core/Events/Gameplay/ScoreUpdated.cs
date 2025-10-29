using ToolsACG.Core.EventBus;

namespace Asteroids.Core.Events.Gameplay
{
    public readonly struct ScoreUpdated : IEvent
    {
        public readonly int Score;

        public ScoreUpdated(int score)
        {
            Score = score;
        }
    }
}