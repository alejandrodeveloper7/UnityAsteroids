using Asteroids.MVC.PauseUI.ScriptableObjects;
using ToolsACG.MVCModulesCreator.Bases;
using Zenject;

namespace Asteroids.MVC.PauseUI.Models
{
    public class PauseUIModel : MVCModelBase
    {
        #region Fields and Properties

        public bool InPause { get; set; }

        #endregion

        #region Events

        //public event Action Something;
        //public event Action<int> SomethingWithParameter;

        #endregion

        #region Constructors

        [Inject]
        public PauseUIModel(SO_PauseUIConfiguration configuration)
        {
            //TODO: Initialize the model with the Configuration SO and other data
        }

        #endregion
    }
}