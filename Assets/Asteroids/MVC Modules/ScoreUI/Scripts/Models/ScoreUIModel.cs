using ACG.Tools.Runtime.MVCModulesCreator.Bases;
using Asteroids.MVC.ScoreUI.ScriptableObjects;
using System;
using UnityEngine;
using Zenject;

namespace Asteroids.MVC.ScoreUI.Models
{
    public class ScoreUIModel : MVCModelBase
    {
        #region Fields and Properties

        [Header("Values")]
        private int _score;

        [Header("Data")]
        private readonly SO_ScoreUIConfiguration _configuration;

        #endregion

        #region Events

        public event Action<int> ScoreUpdated;
        public event Action ScoreRestarted;

        #endregion

        #region Constructors

        [Inject]
        public ScoreUIModel(SO_ScoreUIConfiguration configuration)
        {
            _configuration = configuration;
        }

        #endregion

        #region Score Management

        public void SetScore(int newScore)
        {
            _score = newScore;
            ScoreUpdated?.Invoke(_score);
        }

        public void RestartScore() 
        {
            _score = 0;
            ScoreRestarted?.Invoke();
        }

        #endregion
    }
}