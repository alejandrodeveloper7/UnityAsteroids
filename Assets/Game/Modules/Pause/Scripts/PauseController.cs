using ToolsACG.Utils.Events;
using UnityEngine;

namespace ToolsACG.Scenes.Pause
{
    [RequireComponent(typeof(PauseView))]
    public class PauseController : ModuleController
    {
        #region Private Fields

        private IPauseView _view;
        private PauseModel _data;

        #endregion

        #region Properties

        public PauseModel Model
        {
            get { return _data; }
            set { _data = value; }
        }

        #endregion

        #region Protected Methods

        protected override void Awake()
        {
            _view = GetComponent<IPauseView>();
            base.Awake();
            _data = new PauseModel();

            Initialize();
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

        #region Monobehaviour

        private void OnEnable()
        {
            EventManager.GetGameplayBus().AddListener<PauseKeyClicked>(OnPauseKeyClicked);
        }

        private void OnDisable()
        {
            EventManager.GetGameplayBus().RemoveListener<PauseKeyClicked>(OnPauseKeyClicked);
        }

        #endregion

        #region Bus callbacks

        private void OnPauseKeyClicked(PauseKeyClicked pPauseKeyClicked)
        {
            if (pPauseKeyClicked.InPause)
            {
                Time.timeScale = 0;
                _view.TurnGeneralContainer(true);
            }
            else
            {
                Time.timeScale = 1;
                _view.TurnGeneralContainer(false);
            }
        }

        #endregion

        private void Initialize()
        {
            _view.TurnGeneralContainer(false);
        }
    }
}