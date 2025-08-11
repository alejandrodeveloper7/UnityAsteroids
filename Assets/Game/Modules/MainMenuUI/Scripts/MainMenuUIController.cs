using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ToolsACG.Scenes.MainMenuUI
{
    [RequireComponent(typeof(MainMenuUIView))]
    public class MainMenuUIController : ModuleController
    {
        #region Private Fields

        [Header("View")]
        private MainMenuUIView _view;

        [Header("Model")]
        private MainMenuUIModel _model;
        public MainMenuUIModel Model { get { return _model; } }

        [Header("Data")]
        [SerializeField] SO_MainMenuUIConfiguration _configurationData;
        public SO_MainMenuUIConfiguration ModuleConfigurationData { get { return _configurationData; } }

        [Header("References")]
        [SerializeField] private SelectorController _shipSelectorController;
        [SerializeField] private SelectorController _bulletSelectorController;

        #endregion

        #region Monobehaviour

        protected override void Awake()
        {
            base.Awake();

            GetReferences();
            CreateModel();
            RegisterActions();
        }

        private void Start()
        {
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
            EventManager.UIBus.AddListener<StartGame>(OnStartGame);
            EventManager.UIBus.AddListener<BackToMenuButtonClicked>(OnBackToMenuButtonClicked);
            EventManager.UIBus.AddListener<GameLeaved>(OnGameLeaved);

            Actions["BTN_Play"] = (Action<Button>)((btn) => OnPlayButtonClick(btn));
            Actions["BTN_ExitGame"] = (Action<Button>)((btn) => OnExitGameButtonClick(btn));
            Actions["INF_Username"] = (Action<TMP_InputField>)((inf) => OnUserNameInputFieldValueChanged(inf));
        }

        protected override void UnRegisterActions()
        {
            EventManager.UIBus.RemoveListener<StartGame>(OnStartGame);
            EventManager.UIBus.RemoveListener<BackToMenuButtonClicked>(OnBackToMenuButtonClicked);
            EventManager.UIBus.RemoveListener<GameLeaved>(OnGameLeaved);

            Actions.Clear();
        }

        protected override void GetReferences()
        {
            _view = GetComponent<MainMenuUIView>();
        }

        protected override void Initialize()
        {
            _view.TurnGeneralContainer(false);

            _bulletSelectorController.SetData(ResourcesManager.Instance.GetScriptableObject<BulletsCollection>(ScriptableObjectKeys.BULLET_COLLECTION_KEY).Bullets.Cast<SO_Selectable>().ToList(), 0);
            _shipSelectorController.SetData(ResourcesManager.Instance.GetScriptableObject<ShipsCollection>(ScriptableObjectKeys.SHIP_COLLECTION_KEY).Ships.Cast<SO_Selectable>().ToList(), 0);

            _model.UserName = Environment.UserName;
        }

        protected override void CreateModel()
        {
            _model = new MainMenuUIModel();
        }

        #endregion

        #region UI Elements Actions callbacks

        private void OnExitGameButtonClick(Button pButton)
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private void OnPlayButtonClick(Button pButton)
        {
            PersistentDataManager.SelectedBulletId = _bulletSelectorController.SelectedId;
            PersistentDataManager.SelectedShipId = _shipSelectorController.SelectedId;
            PersistentDataManager.UserName = _model.UserName;

            _view.TurnGeneralContainer(false);

            EventManager.GameplayBus.RaiseEvent(new StartMatch());
        }

        private void OnUserNameInputFieldValueChanged(TMP_InputField pField)
        {
            _model.UserName = pField.text;
        }
        #endregion

        #region BusCallbacks

        private void OnStartGame(StartGame pStartGame)
        {
            _view.EnterTransition(_configurationData.DelayBeforeEnter, _configurationData.DelayBeforeEnter, null);
        }

        private void OnBackToMenuButtonClicked(BackToMenuButtonClicked pBackToMenuButtonClicked)
        {
            _view.EnterTransition(_configurationData.DelayBeforeEnter, _configurationData.DelayBeforeEnter, null);
        }

        private void OnGameLeaved(GameLeaved pGameLeaved)
        {
            _view.EnterTransition(_configurationData.DelayBeforeEnter, _configurationData.DelayBeforeEnter, null);
        }

        #endregion    
    }
}

public readonly struct StartMatch : IEvent
{
}