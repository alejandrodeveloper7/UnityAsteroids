using Asteroids.MVC.PauseUI.Controllers;
using Asteroids.MVC.PauseUI.Models;
using Asteroids.MVC.PauseUI.ScriptableObjects;
using Asteroids.MVC.PauseUI.Views;
using UnityEngine;
using Zenject;

namespace Asteroids.MVC.PauseUI.Installers
{
    public class PauseUISceneInstaller : MonoInstaller
    {
        #region Fields

        [Header("References")]
        [SerializeField] private SO_PauseUIConfiguration _pauseUIConfiguration;

        #endregion

        #region Methods

        public override void InstallBindings()
        {
            Container.Bind<SO_PauseUIConfiguration>().FromInstance(_pauseUIConfiguration).AsSingle();

            Container.Bind<IPauseUIController>().FromComponentSibling();
            Container.Bind<IPauseUIView>().FromComponentSibling();
            Container.Bind<PauseUIModel>().AsSingle();
        }

        #endregion
    }
}