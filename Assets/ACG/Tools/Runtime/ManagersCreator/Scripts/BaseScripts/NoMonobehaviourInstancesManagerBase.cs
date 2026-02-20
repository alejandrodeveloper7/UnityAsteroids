using ACG.Tools.Runtime.ManagersCreator.Interfaces;
using System;
using UnityEngine;
using Zenject;

namespace ACG.Tools.Runtime.ManagersCreator.Bases
{
    public class NoMonobehaviourInstancesManagerBase : IManager, IDisposable, IInitializable
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