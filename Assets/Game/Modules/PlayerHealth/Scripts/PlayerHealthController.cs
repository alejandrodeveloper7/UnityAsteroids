using ToolsACG.Utils.Events;
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
            EventManager.GetGameplayBus().AddListener<StartMatch>(OnStartMatch);

        }

        private void OnDisable()
        {
            EventManager.GetGameplayBus().RemoveListener<StartMatch>(OnStartMatch);
        }

        #endregion

        #region Bus callbacks

        private void OnStartMatch(StartMatch pStartMatch)
        {
            _view.SetViewAlpha(0);
            _view.TurnGeneralContainer(true);
            _view.ViewFadeTransition(1, 0.3f);
        }

        #endregion

        private void Initialize()
        {
            _view.TurnGeneralContainer(false);
        }
    }
}