using Asteroids.Core.Events.GameFlow;
using ToolsACG.Core.EventBus;

namespace Asteroids.Core.Handlers
{
    public class GameFlowEventsHandler
    {
        #region Events management

        public void GameInitialized() 
        { 
            EventBusManager.GameFlowBus.RaiseEvent(new GameInitialized());
        }

        public void RunStarted()
        {
            EventBusManager.GameFlowBus.RaiseEvent(new RunStarted());
        }
        
        public void RunEnded() 
        {
            EventBusManager.GameFlowBus.RaiseEvent(new RunEnded());
        }

        #endregion
    }
}