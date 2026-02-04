using ACG.Scripts.Managers;
using Asteroids.Core.Managers;
using UnityEngine;
using Zenject;

namespace Asteroids.Core.Intallers
{
    [CreateAssetMenu(fileName = "ManagersInstaller", menuName = "Installers/Managers")]
    public class SO_ManagersInstaller : ScriptableObjectInstaller<SO_ManagersInstaller>
    {
        #region Methods

        public override void InstallBindings()
        {         
            //Monobehaviour
            Container.Bind<ISoundManager>().To<SoundManager>().FromComponentInNewPrefabResource("Managers/SoundManager").AsSingle().NonLazy();
            Container.Bind<IPauseManager>().To<PauseManager>().FromComponentInNewPrefabResource("Managers/PauseManager").AsSingle().NonLazy();
            Container.Bind<IInputManager>().To<InputManager>().FromComponentInNewPrefabResource("Managers/InputManager").AsSingle().NonLazy();
            Container.Bind<ICameraFXManager>().To<CameraFXManager>().FromComponentInNewPrefabResource("Managers/CameraFXManager").AsSingle().NonLazy();
            Container.Bind<ICursorManager>().To<CursorManager>().FromComponentInNewPrefabResource("Managers/CursorManager").AsSingle().NonLazy();

            //No Monobehaviour
            Container.Bind<IScreenManager>().To<ScreenManager>().AsSingle().NonLazy();
            Container.Bind<IVFXManager>().To<VFXManager>().AsSingle().NonLazy();
        }

        #endregion
    }
}