using Asteroids.MVC.ScoreUI.Controllers;
using Asteroids.MVC.ScoreUI.Models;
using Asteroids.MVC.ScoreUI.ScriptableObjects;
using Asteroids.MVC.ScoreUI.Views;
using UnityEngine;
using Zenject;

namespace Asteroids.MVC.ScoreUI.Installers
{
    public class ScoreUISceneInstaller : MonoInstaller
    {
        #region Fields

        [Header("References")]
        [SerializeField] private SO_ScoreUIConfiguration _scoreUIConfiguration;

        #endregion

        #region Methods

        public override void InstallBindings()
        {
            Container.Bind<SO_ScoreUIConfiguration>().FromInstance(_scoreUIConfiguration).AsSingle();

            Container.Bind<IScoreUIController>().FromComponentSibling();
            Container.Bind<IScoreUIView>().FromComponentSibling();
            Container.Bind<ScoreUIModel>().AsSingle();
        }

        #endregion
    }
}