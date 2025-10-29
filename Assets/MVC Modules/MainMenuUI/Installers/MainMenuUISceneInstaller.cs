using Asteroids.MVC.MainMenuUI.Controllers;
using Asteroids.MVC.MainMenuUI.Models;
using Asteroids.MVC.MainMenuUI.ScriptableObjects;
using Asteroids.MVC.MainMenuUI.Views;
using UnityEngine;
using Zenject;

namespace Asteroids.MVC.MainMenuUI.Installers
{
    public class MainMenuUISceneInstaller : MonoInstaller
    {
        #region Fields

        [Header("References")]
        [SerializeField] private SO_MainMenuUIConfiguration _mainMenuUIConfiguration;

        #endregion

        #region Methods

        public override void InstallBindings()
        {
            Container.Bind<SO_MainMenuUIConfiguration>().FromInstance(_mainMenuUIConfiguration).AsSingle();

            Container.Bind<IMainMenuUIController>().FromComponentSibling();
            Container.Bind<IMainMenuUIView>().FromComponentSibling();
            Container.Bind<MainMenuUIModel>().AsSingle();
        }

        #endregion
    }
}
