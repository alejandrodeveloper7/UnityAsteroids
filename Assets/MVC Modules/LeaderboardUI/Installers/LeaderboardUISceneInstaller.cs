using Asteroids.MVC.LeaderboardUI.Controllers;
using Asteroids.MVC.LeaderboardUI.Models;
using Asteroids.MVC.LeaderboardUI.ScriptableObjects;
using Asteroids.MVC.LeaderboardUI.Views;
using UnityEngine;
using Zenject;

namespace Asteroids.MVC.LeaderboardUI.Installers
{
    public class LeaderboardUISceneInstaller : MonoInstaller
    {
        #region Fields

        [Header("References")]
        [SerializeField] private SO_LeaderboardUIConfiguration _leaderboardUIConfiguration;
    
        #endregion

        #region Methods

        public override void InstallBindings()
        {
            Container.Bind<SO_LeaderboardUIConfiguration>().FromInstance(_leaderboardUIConfiguration).AsSingle();

            Container.Bind<ILeaderboardUIController>().FromComponentSibling();
            Container.Bind<ILeaderboardUIView>().FromComponentSibling();
            Container.Bind<LeaderboardUIModel>().AsSingle();
        }

        #endregion
    }
}