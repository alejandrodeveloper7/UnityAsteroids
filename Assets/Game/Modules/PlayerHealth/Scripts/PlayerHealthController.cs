using UnityEngine;

namespace ToolsACG.Scenes.PlayerHealth
{
    [RequireComponent(typeof(PlayerHealthView))]
    public class PlayerHealthController : ModuleController
    {
        #region Private Fields

        private IPlayerHealthView _view;
        private PlayerHealthModel _data;
        
        #endregion
        
        #region Properties

        public PlayerHealthModel Model
        {
            get { return _data; }
            set { _data = value; }
        }

        #endregion

        #region Protected Methods

        protected override void Awake()
        {
            _view = GetComponent<IPlayerHealthView>();
            base.Awake();            
            _data = new PlayerHealthModel();
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