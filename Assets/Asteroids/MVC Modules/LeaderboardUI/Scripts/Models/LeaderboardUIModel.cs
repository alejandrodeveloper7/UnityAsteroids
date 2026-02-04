using Asteroids.MVC.LeaderboardUI.ScriptableObjects;
using ToolsACG.MVCModulesCreator.Bases;
using Zenject;

namespace Asteroids.MVC.LeaderboardUI.Models
{
    public class LeaderboardUIModel : MVCModelBase
    {
        #region Fields and Properties

        //Declare your fields and properties here

        #endregion

        #region Events

        //public event Action Something;
        //public event Action<int> SomethingWithParameter;

        #endregion

        #region Constructors

        [Inject]
        public LeaderboardUIModel(SO_LeaderboardUIConfiguration configuration)
        {
            //TODO: Initialize the model with the Configuration SO and other data
        }

        #endregion
    }
}