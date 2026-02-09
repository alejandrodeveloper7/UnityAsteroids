using Asteroids.Core.Controllers;
using Asteroids.Core.Handlers;
using UnityEngine;
using Zenject;

namespace Asteroids.Core.Intallers
{
    [CreateAssetMenu(fileName = "HandlersInstaller", menuName = "Installers/Handlers")]
    public class SO_HandlersInstaller : ScriptableObjectInstaller<SO_HandlersInstaller>
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