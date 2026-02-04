using ACG.Tools.Runtime.MVCModulesCreator.Bases;
using Asteroids.MVC.MainMenuUI.ScriptableObjects;
using System;
using Zenject;

namespace Asteroids.MVC.MainMenuUI.Models
{
    public class MainMenuUIModel : MVCModelBase
    {
        #region Fields and Properties

        private string _userName;
        public string UserName => _userName;

        #endregion

        #region Events

        public event Action<string> UserNameUpdated;

        #endregion

        #region Constructors

        [Inject]
        public MainMenuUIModel(SO_MainMenuUIConfiguration configuration)
        {
            //TODO: Initialize the model with the Configuration SO and other data
        }

        #endregion

        #region Name Management

        public void SetUserName(string newName)
        {
            _userName = newName;
            UserNameUpdated?.Invoke(_userName);
        }

        #endregion
    }
}