using ACG.Tools.Runtime.Pooling.Core;
using ACG.Tools.Runtime.Pooling.Gameplay;
using ACG.Tools.Runtime.Pooling.Managers;
using ACG.Tools.Runtime.Pooling.Pools;
using ACG.Tools.Runtime.Pooling.ScriptableObjects;
using UnityEngine;
using Zenject;

namespace ACG.Tools.Runtime.Pooling.Installers
{
    [CreateAssetMenu(fileName = "PoolingInstaller", menuName = "Installers/Pooling")]
    public class SO_PoolingInstaller : ScriptableObjectInstaller<SO_PoolingInstaller>
    {
        #region Methods

        public override void InstallBindings()
        {
            Container.Bind<SO_FactorySettings>().FromInstance(SO_FactorySettings.Instance);

            Container.Bind<FactoryManager>().AsSingle().NonLazy();
            Container.Bind<GameObjectPool>().AsTransient();

            Container.Bind<AudioSourcePoolManager>().AsSingle();
            Container.Bind<Sound3DPoolManager>().AsSingle();
            Container.Bind<GameObjectPoolManager>().AsSingle();

            Container.Bind<PooledGameObjectController>().FromComponentSibling();
        }

        #endregion
    }
}