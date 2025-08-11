using System.Threading.Tasks;
using UnityEngine;

namespace ToolsACG.Scenes.PlayerHealthBarUI
{
    [RequireComponent(typeof(PlayerHealthBarUIView))]
    public class PlayerHealthBarUIController : ModuleController
    {
        #region Private Fields

        [Header("Model")]
        private PlayerHealthBarUIView _view;

        [Header("Model")]
        private PlayerHealthBarUIModel _model;
        public PlayerHealthBarUIModel Model { get { return _model; } }

        [Header("Data")]
        [SerializeField] SO_PlayerHealthBarUIConfiguration _configurationData;
        public SO_PlayerHealthBarUIConfiguration ModuleConfigurationData { get { return _configurationData; } }
        private PlayerSettings _playerSettings;

        #endregion

        #region Monobehaviour

        protected override void Awake()
        {
            base.Awake();

            GetReferences();
            CreateModel();
            RegisterActions();
            Initialize();
        }

        private void OnDestroy()
        {
            UnRegisterActions();
        }

        #endregion

        #region Initialization

        protected override void RegisterActions()
        {
            EventManager.GameplayBus.AddListener<StartMatch>(OnStartMatch);
            EventManager.GameplayBus.AddListener<PlayerDamaged>(OnPlayerDamaged);
            EventManager.GameplayBus.AddListener<PlayerDead>(OnPlayerDead);
            EventManager.GameplayBus.AddListener<ShieldStateChanged>(OnShieldStateChanged);
            EventManager.UIBus.AddListener<GameLeaved>(OnGameLeaved);

            //Actions["MyButton"] = (Action<Button>)((btn) => Debug.Log($"Clicked: {btn.name}"));
            //Actions["MyToggle"] = (Action<Toggle>)((tgl) => Debug.Log($"Toggle isOn: {tgl.isOn}"));
            //Actions["MySlider"] = (Action<Slider>)((sld) => Debug.Log($"Slider value: {sld.value}"));
            //Actions["MyInputField"] = (Action<TMP_InputField>)((imf) => Debug.Log($"InputField text: {imf.text}"));
            //Actions["MyDropdown"] = (Action<TMP_Dropdown>)((dpd) => Debug.Log($"Dropdown value: {dpd.value}"));
            //Actions["MyScrollbar"] = (Action<Scrollbar>)((scb) => Debug.Log($"ScrollBar value: {scb.value}"));
            //Actions["MyScrollRect"] = (Action<ScrollRect>)((scr) => Debug.Log($"ScrollRect verticalNormalizedPosition: {scr.verticalNormalizedPosition}"));
        }

        protected override void UnRegisterActions()
        {
            EventManager.GameplayBus.RemoveListener<StartMatch>(OnStartMatch);
            EventManager.GameplayBus.RemoveListener<PlayerDamaged>(OnPlayerDamaged);
            EventManager.GameplayBus.RemoveListener<PlayerDead>(OnPlayerDead);
            EventManager.GameplayBus.RemoveListener<ShieldStateChanged>(OnShieldStateChanged);
            EventManager.UIBus.RemoveListener<GameLeaved>(OnGameLeaved);

            Actions.Clear();
        }

        protected override void GetReferences()
        {
            _view = GetComponent<PlayerHealthBarUIView>();
            _playerSettings = ResourcesManager.Instance.GetScriptableObject<PlayerSettings>(ScriptableObjectKeys.PLAYER_SETTINGS_KEY);
        }

        protected override void Initialize()
        {
            _view.TurnGeneralContainer(false);
            _view.SetMaxPosibleHealthValue(_playerSettings.HealthPoints);
        }

        protected override void CreateModel()
        {
            _model = new PlayerHealthBarUIModel(_configurationData, _playerSettings);
        }

        #endregion

        #region UI Elements Actions callbacks

        // TODO: Declare here the callbacks of the UI interactios registered in the Actions Dictionary

        #endregion

        #region Bus callbacks

        private void OnStartMatch(StartMatch pStartMatch)
        {
            _model.StopShieldRecoveryProcess();
            _model.CurrentHealth = _playerSettings.HealthPoints;
            _model.CurrentShieldSliderValue = _configurationData.ShielSliderMaxValue;
            _view.EnterTransition(_configurationData.DelayBeforeEnter, _configurationData.FadeTransitionDuration, null);
        }

        private void OnPlayerDamaged(PlayerDamaged pPlayerDamaged)
        {
            _model.CurrentHealth = pPlayerDamaged.Health;
        }

        private void OnPlayerDead(PlayerDead pPlayerDead)
        {
            _model.StopShieldRecoveryProcess();
            _view.ExitTransiion(_configurationData.DelayBeforeExit, _configurationData.FadeTransitionDuration, null);
        }

        private async void OnShieldStateChanged(ShieldStateChanged pShieldStateChanged)
        {
            if (pShieldStateChanged.Active)
                return;

            _view.DoShielSliderTransition(_configurationData.ShieldLostSliderMinValue, _configurationData.ShieldSliderTransitionDuration);
            await Task.Delay((int)(_configurationData.ShieldSliderTransitionDuration * 1000));
            _model.StartShieldRecoveryProcess();
        }

        private void OnGameLeaved(GameLeaved pGameLeaved)
        {
            _model.StopShieldRecoveryProcess();
            _view.ExitTransiion(_configurationData.DelayBeforeExit, _configurationData.FadeTransitionDuration, null);
        }

        #endregion      
    }
}