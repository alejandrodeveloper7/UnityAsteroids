using Asteroids.Gameplay.Asteroids.Factorys;
using Asteroids.Gameplay.Bullets.Factorys;
using Asteroids.Gameplay.FloatingText.Factorys;
using Asteroids.Gameplay.Player.Factorys;
using UnityEngine;
using Zenject;

namespace Asteroids.Core.Intallers
{
    [CreateAssetMenu(fileName = "FactoriesInstaller", menuName = "Installers/Factories")]
    public class SO_FactoriesInstaller : ScriptableObjectInstaller<SO_FactoriesInstaller>
    {
        #region Methods

        public override void InstallBindings()
        {
            Container.Bind<AsteroidFactory>().AsSingle();
            Container.Bind<PlayerFactory>().AsSingle();
            Container.Bind<BulletFactory>().AsSingle();
            Container.Bind<FloatingTextFactory>().AsSingle();
        }

        #endregion
    }
}