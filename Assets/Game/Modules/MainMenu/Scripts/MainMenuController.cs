using UnityEngine;

namespace ToolsACG.Scenes.MainMenu
{
    [RequireComponent(typeof(MainMenuView))]
    public class MainMenuController : ModuleController
    {
        #region Private Fields

        private IMainMenuView _view;
        private MainMenuModel _data;
        
        #endregion
        
        #region Properties

        public MainMenuModel Model
        {
            get { return _data; }
            set { _data = value; }
        }

        #endregion

        #region Protected Methods

        protected override void Awake()
        {
            _view = GetComponent<IMainMenuView>();
            base.Awake();            
            _data = new MainMenuModel();
        }

        protected override void RegisterActions()
        {
            // TODO: initialize dictionaries with actions for buttons, toggles and dropdowns.      
        }

        protected override void SetData()
        {
            // TODO: initialize model with services data (if it's not initialized externally using Data property).
            // TODO: call view methods to display data.
        }

        #endregion           
    }
}