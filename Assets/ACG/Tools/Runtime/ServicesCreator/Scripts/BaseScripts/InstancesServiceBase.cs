using ACG.Tools.Runtime.ServicesCreator.Interfaces;
using System;
using UnityEngine;
using Zenject;

namespace ACG.Tools.Runtime.ServicesCreator.Bases
{
    public abstract class InstancesServiceBase : IService, IDisposable, IInitializable
    {
        private void OnAppQuit()
        {
            Dispose();
        }

        public virtual void Initialize()
        {
            Application.quitting += OnAppQuit;
        }

        public virtual void Dispose()
        {
            Application.quitting -= OnAppQuit;
        }
    }
}