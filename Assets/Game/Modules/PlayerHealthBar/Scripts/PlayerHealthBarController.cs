using DG.Tweening;
using System.Runtime.CompilerServices;
using ToolsACG.Utils.Events;
using UnityEditor;
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
        private Tween _shieldChargeTween;
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
            EventManager.GetGameplayBus().AddListener<PlayerDamaged>(OnPlayerDamaged);
            EventManager.GetGameplayBus().AddListener<PlayerDead>(OnPlayerDead);
            EventManager.GetGameplayBus().AddListener<ShieldStateChanged>(OnShieldStateChanged);

        }

        private void OnDisable()
        {
            EventManager.GetGameplayBus().RemoveListener<StartMatch>(OnStartMatch);
            EventManager.GetGameplayBus().RemoveListener<PlayerDamaged>(OnPlayerDamaged);
            EventManager.GetGameplayBus().RemoveListener<PlayerDead>(OnPlayerDead);
            EventManager.GetGameplayBus().RemoveListener<ShieldStateChanged>(OnShieldStateChanged);
        }

        #endregion

        #region Bus callbacks

        private void OnStartMatch(StartMatch pStartMatch)
        {
            _view.SetCurrentHealth(_playersettings.HealthPoints);
            _view.SetViewAlpha(0);
            _view.SetShieldSliderValue(100);
            _view.TurnGeneralContainer(true);
            _view.ViewFadeTransition(1, 0.3f);
        }

        private void OnPlayerDamaged(PlayerDamaged pPlayerDamaged)
        {
            _view.SetCurrentHealth(pPlayerDamaged.Health);
        }

        private void OnPlayerDead(PlayerDead pPlayerDead)
        {
            _shieldChargeTween?.Kill();
            _view.ViewFadeTransition(0, 0.3f);
        }

        private void OnShieldStateChanged(ShieldStateChanged pShieldStateChanged)
        {
            if (pShieldStateChanged.Active)
                return;

            _view.DoShielSliderTransition(_playersettings.ShieldSliderMinValue, 0.8f);

            float currentValue = _playersettings.ShieldSliderMinValue;

            _shieldChargeTween = DOTween.To(() => currentValue, x => currentValue = x, 100, _playersettings.ShieldRecoveryTime)
                .SetEase(Ease.Linear)
                .OnUpdate(() =>
                {
                    _view.SetShieldSliderValue(currentValue);
                })
                .OnComplete(() =>
                {
                    _view.SetShieldSliderValue(100);
                    EventManager.GetGameplayBus().RaiseEvent(new ShieldStateChanged() { Active = true });
                });

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