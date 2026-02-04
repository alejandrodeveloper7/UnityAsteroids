using Asteroids.MVC.PlayerHealthBarUI.Controllers;
using Asteroids.MVC.PlayerHealthBarUI.Models;
using Asteroids.MVC.PlayerHealthBarUI.ScriptableObjects;
using Asteroids.MVC.PlayerHealthBarUI.Views;
using UnityEngine;
using Zenject;

namespace Asteroids.MVC.PlayerHealthBarUI.Installers
{
    public class PlayerHealthBarUISceneInstaller : MonoInstaller
    {
        #region Fields

        [Header("References")]
        [SerializeField] private SO_PlayerHealthBarUIConfiguration _playerHealthBarUIConfiguration;

        #endregion

        #region Methods

        public override void InstallBindings()
        {
            Container.Bind<SO_PlayerHealthBarUIConfiguration>().FromInstance(_playerHealthBarUIConfiguration).AsSingle();

            Container.Bind<IPlayerHealthBarUIController>().FromComponentSibling();
            Container.Bind<IPlayerHealthBarUIView>().FromComponentSibling();
            Container.Bind<PlayerHealthBarUIModel>().AsSingle();
        }

        #endregion
    }
}