using Asteroids.MVC.ScoreUI.ScriptableObjects;
using ToolsACG.MVCModulesCreator.Bases;
using System;
using UnityEngine;
using Zenject;

namespace Asteroids.MVC.ScoreUI.Models
{
    public class ScoreUIModel : MVCModelBase
    {
        #region Fields and Properties

        [Header("Score")]
        private int _score; 

        #endregion

        #region Events

        public event Action<int> ScoreUpdated;
        public event Action ScoreRestarted;

        #endregion

        #region Constructors

        [Inject]
        public ScoreUIModel(SO_ScoreUIConfiguration configuration)
        {
            //TODO: Initialize the model with the Configuration SO and other data
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