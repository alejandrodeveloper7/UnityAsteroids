using ToolsACG.ServicesCreator.Interfaces;
using UnityEngine;

namespace ToolsACG.ServicesCreator.Bases
{
    public abstract class InstancesServiceBase : IService
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