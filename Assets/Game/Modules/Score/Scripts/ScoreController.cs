using UnityEngine;

namespace ToolsACG.Scenes.Score
{
    [RequireComponent(typeof(ScoreView))]
    public class ScoreController : ModuleController
    {
        #region Private Fields

        private IScoreView _view;
        private ScoreModel _data;
        
        #endregion
        
        #region Properties

        public ScoreModel Model
        {
            get { return _data; }
            set { _data = value; }
        }

        #endregion

        #region Protected Methods

        protected override void Awake()
        {
            _view = GetComponent<IScoreView>();
            base.Awake();            
            _data = new ScoreModel();
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