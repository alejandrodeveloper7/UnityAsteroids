using Asteroids.Gameplay.Asteroids.Spawners;
using Asteroids.Gameplay.Bullets.Spawners;
using Asteroids.Gameplay.FloatingText.Spawners;
using Asteroids.Gameplay.Player.Spawners;
using UnityEngine;
using Zenject;

namespace Asteroids.Core.Intallers
{
    [CreateAssetMenu(fileName = "GlobalSpawnersInstaller", menuName = "Installers/GlobalSpawners")]
    public class SO_GlobalSpawnersInstaller : ScriptableObjectInstaller<SO_GlobalSpawnersInstaller>
    {
        #region Methods

        public override void InstallBindings()
        {
            Container.Bind<AsteroidSpawner>().AsSingle();
            Container.Bind<PlayerSpawner>().AsSingle();
            Container.Bind<BulletSpawner>().AsSingle();
            Container.Bind<FloatingTextSpawner>().AsSingle();
        }

        #endregion
    }
}