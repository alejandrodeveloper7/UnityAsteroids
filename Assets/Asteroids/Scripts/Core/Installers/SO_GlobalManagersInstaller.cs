using ACG.Scripts.Managers;
using Asteroids.Core.Managers;
using UnityEngine;
using Zenject;

namespace Asteroids.Core.Intallers
{
    [CreateAssetMenu(fileName = "GlobalManagersInstaller", menuName = "Installers/GlobalManagers")]
    public class SO_GlobalManagersInstaller : ScriptableObjectInstaller<SO_GlobalManagersInstaller>
    {
        #region Methods

        public override void InstallBindings()
        {
            //Monobehaviour
            Container.BindInterfacesAndSelfTo<SoundManager>().FromComponentInNewPrefabResource("Managers/SoundManager").AsSingle().NonLazy();      
            Container.BindInterfacesAndSelfTo<PauseManager>().FromComponentInNewPrefabResource("Managers/PauseManager").AsSingle().NonLazy();      
            Container.BindInterfacesAndSelfTo<InputManager>().FromComponentInNewPrefabResource("Managers/InputManager").AsSingle().NonLazy();      
            Container.BindInterfacesAndSelfTo<CameraFXManager>().FromComponentInNewPrefabResource("Managers/CameraFXManager").AsSingle().NonLazy();      
            Container.BindInterfacesAndSelfTo<CursorManager>().FromComponentInNewPrefabResource("Managers/CursorManager").AsSingle().NonLazy();

            //No Monobehaviour
            Container.BindInterfacesAndSelfTo<ScreenManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<VFXManager>().AsSingle().NonLazy();
        }

        #endregion
    }
}