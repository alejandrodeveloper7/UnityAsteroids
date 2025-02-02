using ToolsACG.Utils.Events;
using UnityEngine;

namespace ToolsACG.Scenes.PlayerHealth
{
    [RequireComponent(typeof(PlayerHealthBarView))]
    public class PlayerHealthBarController : ModuleController
    {
        #region Private Fields

        private IPlayerHealthBarView _view;
        private PlayerHealthBarModel _data;

        private PlayerSettings _playersettings;

        #endregion

        #region Properties

        public PlayerHealthBarModel Model
        {
            get { return _data; }
            set { _data = value; }
        }

        #endregion

        #region Protected Methods

        protected override void Awake()
        {
            _view = GetComponent<IPlayerHealthBarView>();
            base.Awake();
            _data = new PlayerHealthBarModel();

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
            EventManager.GetGameplayBus().AddListener<PlayerHealthUpdated>(OnPlayerHealthUpdated);
            EventManager.GetGameplayBus().AddListener<PlayerDead>(OnPlayerDead);

        }

        private void OnDisable()
        {
            EventManager.GetGameplayBus().RemoveListener<StartMatch>(OnStartMatch);
            EventManager.GetGameplayBus().RemoveListener<PlayerHealthUpdated>(OnPlayerHealthUpdated);
            EventManager.GetGameplayBus().RemoveListener<PlayerDead>(OnPlayerDead);
        }

        #endregion

        #region Bus callbacks

        private void OnStartMatch(StartMatch pStartMatch)
        {
            _view.SetCurrentHealth(_playersettings.HealthPoints);
            _view.SetViewAlpha(0);
            _view.TurnGeneralContainer(true);
            _view.ViewFadeTransition(1, 0.3f);
        }

        private void OnPlayerHealthUpdated(PlayerHealthUpdated pPlayerHealthUpdated)
        {
            _view.SetCurrentHealth(pPlayerHealthUpdated.Health);
        }

        private void OnPlayerDead(PlayerDead pPlayerDead) 
        {
            _view.ViewFadeTransition(0, 0.3f);
        }

        #endregion

        private void Initialize()
        {
            _playersettings = ResourcesManager.Instance.PlayerSettings;
            _view.TurnGeneralContainer(false);

            _view.SetHealthPointsSprites(_playersettings.HealthPointSprite, _playersettings.emptyHealtPointSprite);
            _view.SetShieldSliderSprites(_playersettings.ShieldBarSprite, _playersettings.FullShieldBarSprite);
            _view.SetMaxHealth(_playersettings.HealthPoints);
        }
    }
}