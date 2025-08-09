using DG.Tweening;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace ToolsACG.Scenes.PlayerHealth
{
    [RequireComponent(typeof(PlayerHealthBarView))]
    public class PlayerHealthBarController : ModuleController
    {
        #region Private Fields

        private IPlayerHealthBarView _view;
        [SerializeField] private PlayerHealthBarModel _data;
        [Space]
        private PlayerSettings _playersettings;
        private Tween _shieldChargeTween;

        #endregion

        #region Protected Methods

        protected override void Awake()
        {
            base.Awake();

            GetReferences();
            Initialize();
        }

        protected override void RegisterActions()
        {
            // TODO: initialize dictionaries with actions for buttons, toggles, sliders, input fields and dropdowns.      
        }

        protected override void UnRegisterActions()
        {
            // TODO: Unregister listeners and dictionarie actions.      
        }

        protected override void GetReferences()
        {
            _view = GetComponent<IPlayerHealthBarView>();
        }

        protected override void Initialize()
        {
            _playersettings = ResourcesManager.Instance.GetScriptableObject<PlayerSettings>(ScriptableObjectKeys.PLAYER_SETTINGS_KEY);

            _view.SetHealthPointsSprites(_data.HealthPointSprite, _data.emptyHealtPointSprite);
            _view.SetShieldSliderSprites(_data.ShieldBarSprite, _data.FullShieldBarSprite);

            _view.SetMaxHealth(_playersettings.HealthPoints);
            _view.TurnGeneralContainer(false);
        }

        #endregion

        #region Monobehaviour

        private void OnEnable()
        {
            EventManager.GameplayBus.AddListener<StartMatch>(OnStartMatch);
            EventManager.GameplayBus.AddListener<PlayerDamaged>(OnPlayerDamaged);
            EventManager.GameplayBus.AddListener<PlayerDead>(OnPlayerDead);
            EventManager.GameplayBus.AddListener<ShieldStateChanged>(OnShieldStateChanged);
            EventManager.UIBus.AddListener<GameLeaved>(OnGameLeaved);
        }

        private void OnDisable()
        {
            EventManager.GameplayBus.RemoveListener<StartMatch>(OnStartMatch);
            EventManager.GameplayBus.RemoveListener<PlayerDamaged>(OnPlayerDamaged);
            EventManager.GameplayBus.RemoveListener<PlayerDead>(OnPlayerDead);
            EventManager.GameplayBus.RemoveListener<ShieldStateChanged>(OnShieldStateChanged);
            EventManager.UIBus.RemoveListener<GameLeaved>(OnGameLeaved);
        }

        #endregion

        #region Bus callbacks

        private void OnStartMatch(StartMatch pStartMatch)
        {
            RestartHealthAndShield();
            DoEntranceWithDelay(0);
        }

        private void OnPlayerDamaged(PlayerDamaged pPlayerDamaged)
        {
            _view.SetCurrentHealth(pPlayerDamaged.Health);
        }

        private void OnPlayerDead(PlayerDead pPlayerDead)
        {
            _shieldChargeTween?.Kill();
            DoExitWithDelay(0);
        }

        private void OnShieldStateChanged(ShieldStateChanged pShieldStateChanged)
        {
            if (pShieldStateChanged.Active)
                return;

            StartLostAndRecoveryShieldProcess();
        }

        private void OnGameLeaved(GameLeaved pGameLeaved)
        {
            _shieldChargeTween?.Kill();
            DoExitWithDelay(0);
        }

        #endregion

        #region Navigation

        private async void DoEntranceWithDelay(float pDelay, Action pOnComplete = null)
        {
            await Task.Delay((int)(pDelay * 1000));
            View.SetViewAlpha(0);
            _view.TurnGeneralContainer(true);
            View.DoFadeTransition(1, _data.FadeTransitionDuration);
            await Task.Delay((int)(_data.FadeTransitionDuration * 1000));
            pOnComplete?.Invoke();
        }

        private async void DoExitWithDelay(float pDelay, Action pOnComplete = null)
        {
            await Task.Delay((int)(pDelay * 1000));
            View.SetViewAlpha(1);
            View.DoFadeTransition(0, _data.FadeTransitionDuration);
            await Task.Delay((int)(_data.FadeTransitionDuration * 1000));
            _view.TurnGeneralContainer(false);
            pOnComplete?.Invoke();
        }

        #endregion

        #region Functionality

        private void RestartHealthAndShield()
        {
            _view.SetCurrentHealth(_playersettings.HealthPoints);
            _view.SetShieldSliderValue(100);
        }

        private async void StartLostAndRecoveryShieldProcess()
        {
            _view.DoShielSliderTransition(_data.ShieldLostSliderMinValue, _data.ShieldSliderTransitionDuration);
            await Task.Delay((int)(_data.ShieldSliderTransitionDuration * 1000));

            float currentValue = _data.ShieldLostSliderMinValue;

            _shieldChargeTween = DOTween.To(() => currentValue, x => currentValue = x, 100, _playersettings.ShieldRecoveryTime)
                .SetEase(Ease.Linear)
                .OnUpdate(() =>
                {
                    _view.SetShieldSliderValue(currentValue);
                })
                .OnComplete(() =>
                {
                    _view.SetShieldSliderValue(100);
                    EventManager.GameplayBus.RaiseEvent(new ShieldStateChanged(true));
                });
        }

        #endregion
    }
}