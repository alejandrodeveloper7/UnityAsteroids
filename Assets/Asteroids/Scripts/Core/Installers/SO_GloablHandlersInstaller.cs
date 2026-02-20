using Asteroids.Core.Controllers;
using Asteroids.Core.Handlers;
using UnityEngine;
using Zenject;

namespace Asteroids.Core.Intallers
{
    [CreateAssetMenu(fileName = "GlobalHandlersInstaller", menuName = "Installers/GloablHandlers")]
    public class SO_GloablHandlersInstaller : ScriptableObjectInstaller<SO_GloablHandlersInstaller>
    {
        #region Methods

        public override void InstallBindings()
        {
            Container.Bind<GameFlowEventsHandler>().AsSingle();
            Container.Bind<ScenesHandler>().AsSingle();
        }

        #endregion
    }
}