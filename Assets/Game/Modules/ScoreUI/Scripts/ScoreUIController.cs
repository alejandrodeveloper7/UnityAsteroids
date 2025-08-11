using UnityEngine;

namespace ToolsACG.Scenes.ScoreUI
{
    [RequireComponent(typeof(ScoreUIView))]
    public class ScoreUIController : ModuleController
    {
        #region Private Fields

        [Header("View")]
        private ScoreUIView _view;

        [Header("Model")]
        private ScoreUIModel _model; 
        public ScoreUIModel Model { get { return _model;}}

        [Header("Data")]
        [SerializeField] SO_ScoreUIConfiguration _configurationData;   
        public SO_ScoreUIConfiguration ModuleConfigurationData { get { return _configurationData;}}
        
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
            // TODO: Initialize listeners and Actions for buttons, toggles, sliders...    
            
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
            // TODO: Unregister listeners.     
            
            Actions.Clear();
        }       

        protected override void GetReferences()
        {
            _view=GetComponent<ScoreUIView>();
        }

        protected override void Initialize()
        {
            _view.TurnGeneralContainer(false);
        }

        protected override void CreateModel()
        {
            _model = new ScoreUIModel();
        }

        #endregion

        #region UI Elements Actions callbacks

        // TODO: Declare here the callbacks of the UI interactios registered in the Actions Dictionary

        #endregion
               

        #region Monobehaviour

        private void OnEnable()
        {
            EventManager.GameplayBus.AddListener<StartMatch>(OnStartMatch);
            EventManager.GameplayBus.AddListener<AsteroidDestroyed>(OnAsteroidDestroyed);
            EventManager.GameplayBus.AddListener<PlayerDead>(OnPlayerDead);
            EventManager.UIBus.AddListener<BackToMenuButtonClicked>(OnBackToMenuButtonClicked);
            EventManager.UIBus.AddListener<GameLeaved>(OnGameLeaved);
        }

        private void OnDisable()
        {
            EventManager.GameplayBus.RemoveListener<StartMatch>(OnStartMatch);
            EventManager.GameplayBus.RemoveListener<AsteroidDestroyed>(OnAsteroidDestroyed);
            EventManager.GameplayBus.RemoveListener<PlayerDead>(OnPlayerDead);
            EventManager.UIBus.RemoveListener<BackToMenuButtonClicked>(OnBackToMenuButtonClicked);
            EventManager.UIBus.RemoveListener<GameLeaved>(OnGameLeaved);
        }

        #endregion

        #region Bus callbacks

        private void OnStartMatch(StartMatch pStartMatch)
        {
            _model.SetAlive(true);
            _model.Score=0;
            _view.EnterTransition(_configurationData.DelayBeforeEnter,_configurationData.FadeTransitionDuration, null);
        }

        private void OnAsteroidDestroyed(AsteroidDestroyed pAsteroidDestroyed)
        {
            if (_model.Alive is false)
                return;

            _model.Score+=pAsteroidDestroyed.AsteroidData.PointsValue;
        }

        private void OnPlayerDead(PlayerDead pPlayerDead)
        {
            _model.SetAlive(false);
            PersistentDataManager.LastScore = _model.Score;
        }

        private void OnBackToMenuButtonClicked(BackToMenuButtonClicked pBackToMenuButtonClicked)
        {
            _view.EnterTransition(_configurationData.DelayBeforeExit, _configurationData.FadeTransitionDuration, null);
        }

        private void OnGameLeaved(GameLeaved pGameLeaved)
        {
            _model.SetAlive(false);
            _view.EnterTransition(_configurationData.DelayBeforeExit, _configurationData.FadeTransitionDuration, null);
        }

        #endregion

    }
}