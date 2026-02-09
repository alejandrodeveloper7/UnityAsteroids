using Asteroids.Core.Controllers;
using UnityEngine;
using Zenject;

namespace Asteroids.Core.Intallers
{
    [CreateAssetMenu(fileName = "ControllersInstaller", menuName = "Installers/Constrollers")]
    public class SO_ControllersInstaller : ScriptableObjectInstaller<SO_ControllersInstaller>
    {
        #region Methods

        public override void InstallBindings()
        {
            //Controllers
            Container.Bind<LeaderboardController>().AsSingle();
            Container.Bind<AsteroidsController>().AsSingle();
            Container.Bind<ScoreController>().AsSingle();
            Container.Bind<PlayersController>().AsSingle();
        }

        #endregion
    }
}