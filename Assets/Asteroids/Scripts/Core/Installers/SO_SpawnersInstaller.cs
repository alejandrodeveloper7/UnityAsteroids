using Asteroids.Gameplay.Asteroids.Spawners;
using Asteroids.Gameplay.Bullets.Spawners;
using Asteroids.Gameplay.FloatingText.Spawners;
using Asteroids.Gameplay.Player.Spawners;
using UnityEngine;
using Zenject;

namespace Asteroids.Core.Intallers
{
    [CreateAssetMenu(fileName = "SpawnersInstaller", menuName = "Installers/Spawners")]
    public class SO_SpawnersInstaller : ScriptableObjectInstaller<SO_SpawnersInstaller>
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