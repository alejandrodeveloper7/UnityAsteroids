using ToolsACG.ManagersCreator.Interfaces;
using UnityEngine;

namespace ToolsACG.ManagersCreator.Bases
{
    public class NoMonobehaviourInstancesManagerBase : IManager 
    {     
        #region Initilization

        public virtual void Initialize()
        {
            Application.quitting += OnApplicationQuit;
        }

        public virtual void Dispose()
        {
            Application.quitting -= OnApplicationQuit;
        }

        #endregion

        #region Event Callbacks

        private void OnApplicationQuit()
        {
            Dispose();
        }

        #endregion
    }
}