using ToolsACG.Pooling.Core;
using ToolsACG.Pooling.Managers;
using ToolsACG.Pooling.Pools;
using UnityEngine;
using Zenject;

namespace Asteroids.Core.Intallers
{
    [CreateAssetMenu(fileName = "PoolingInstaller", menuName = "Installers/Pooling")]
    public class SO_PoolingInstaller : ScriptableObjectInstaller<SO_PoolingInstaller>
    {
        #region Methods

        public override void InstallBindings()
        {
            Container.Bind<FactoryManager>().AsSingle().NonLazy();
            Container.Bind<GameObjectPool>().AsTransient();

            Container.Bind<AudioSourcePoolManager>().AsSingle();
            Container.Bind<Sound3DPoolManager>().AsSingle();
            Container.Bind<GameObjectPoolManager>().AsSingle();
        }

        #endregion
    }
}