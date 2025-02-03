using System.Linq;
using UnityEngine;
using System;
using ToolsACG.Utils.Events;
using System.Threading.Tasks;


#if UNITY_EDITOR
using UnityEditor;
#endif


namespace ToolsACG.Scenes.MainMenu
{
    [RequireComponent(typeof(MainMenuView))]
    public class MainMenuController : ModuleController
    {
        #region Private Fields

        private IMainMenuView _view;
        private MainMenuModel _data;

        [SerializeField] private SelectorController _shipSelectorController;
        [SerializeField] private SelectorController _bulletSelectorController;
  
        #endregion

        #region Properties

        public MainMenuModel Model
        {
            get { return _data; }
            set { _data = value; }
        }

        #endregion

        #region Protected Methods

        protected override void Awake()
        {
            _view = GetComponent<IMainMenuView>();
            base.Awake();            
            _data = new MainMenuModel();

            Initialize();
        }

        protected override void RegisterActions()
        {
            Actions.Add("BTN_Play", OnPlayButtonClick);
            Actions.Add("BTN_ExitGame", OnExitGameButtonClick);
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
            EventManager.GetUiBus().AddListener<StartGame>(OnStartGame);
            EventManager.GetUiBus().AddListener<BackToMenuButtonClicked>(OnBackToMenuButtonClicked);

        }

        private void OnDisable()
        {
            EventManager.GetUiBus().RemoveListener<StartGame>(OnStartGame);
            EventManager.GetUiBus().AddListener<BackToMenuButtonClicked>(OnBackToMenuButtonClicked);
        }

        #endregion

        #region BusCallbacks

        private async void OnStartGame(StartGame pStartGame) 
        {
            _view.SetViewAlpha(0);
            _view.TurnGeneralContainer(true);
            await Task.Delay(700);
            _view.ViewFadeTransition(1, 0.3f);
        }

        private async void OnBackToMenuButtonClicked(BackToMenuButtonClicked pBackToMenuButtonClicked) 
        {
            await Task.Delay(300);
            _view.SetViewAlpha(0);
            _view.TurnGeneralContainer(true);
            _view.ViewFadeTransition(1, 0.3f);
        }

        #endregion

        #region Button Callbacks

        private void OnExitGameButtonClick() 
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private void OnPlayButtonClick() 
        {
            PersistentDataManager.SelectedBulletId = _bulletSelectorController.SelectedId;
            PersistentDataManager.SelectedShipId = _shipSelectorController.SelectedId;
            PersistentDataManager.UserName =_view.UserName;

            _view.TurnGeneralContainer(false);

            EventManager.GetGameplayBus().RaiseEvent(new StartMatch());
        }

#endregion

        private void Initialize()
        {
            _bulletSelectorController.SetData(ResourcesManager.Instance.BulletSettings.Bullets.Cast<SO_BaseElement>().ToList(), 0);
            _shipSelectorController.SetData(ResourcesManager.Instance.ShipSettings.Ships.Cast<SO_BaseElement>().ToList(), 0);
            _view.TurnGeneralContainer(false);
            _view.SetUserNameValue(Environment.UserName);
        }
    }
}

public class StartMatch : IEvent 
{
}