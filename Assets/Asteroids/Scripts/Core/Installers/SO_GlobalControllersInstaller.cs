using Asteroids.Core.Controllers;
using UnityEngine;
using Zenject;

namespace Asteroids.Core.Intallers
{
    [CreateAssetMenu(fileName = "GlobalControllersInstaller", menuName = "Installers/GlobalConstrollers")]
    public class SO_GlobalControllersInstaller : ScriptableObjectInstaller<SO_GlobalControllersInstaller>
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