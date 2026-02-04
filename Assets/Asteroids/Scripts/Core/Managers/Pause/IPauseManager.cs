using System;

namespace Asteroids.Core.Managers
{
    public interface IPauseManager 
    {
        event Action GamePaused;
        event Action GameResumed;
        
        bool IsPaused { get; }

        void TogglePause();
        void Pause();
        void Resume();
    }
}