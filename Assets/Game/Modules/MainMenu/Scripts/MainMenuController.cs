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
        [SerializeField] private MainMenuModel _data;
        [Space]
        [SerializeField] private SelectorController _shipSelectorController;
        [SerializeField] private SelectorController _bulletSelectorController;
  
        #endregion

        #region Protected Methods

        protected override void Awake()
        {
            base.Awake();

            GetReferences();
            Initialize();
            RegisterActions();
        }

        protected override void RegisterActions()
        {
            Actions.Add("BTN_Play", OnPlayButtonClick);
            Actions.Add("BTN_ExitGame", OnExitGameButtonClick);
        }

        protected override void UnRegisterActions()
        {
            // TODO: Unregister listeners and dictionarie actions.      
        }

        protected override void GetReferences()
        {
            _view = GetComponent<IMainMenuView>();
        }

        protected override void Initialize()
        {
            _bulletSelectorController.SetData(ResourcesManager.Instance.GetScriptableObject<BulletsCollection>(ScriptableObjectKeys.BULLET_COLLECTION_KEY).Bullets.Cast<SO_Selectable>().ToList(), 0);
            _shipSelectorController.SetData(ResourcesManager.Instance.GetScriptableObject<ShipsCollection>(ScriptableObjectKeys.SHIP_COLLECTION_KEY).Ships.Cast<SO_Selectable>().ToList(), 0);

            _view.TurnGeneralContainer(false);
            _view.SetUserNameValue(Environment.UserName);
        }

        #endregion

        #region Monobehaviour

        private void OnEnable()
        {
            EventManager.UIBus.AddListener<StartGame>(OnStartGame);
            EventManager.UIBus.AddListener<BackToMenuButtonClicked>(OnBackToMenuButtonClicked);
            EventManager.UIBus.AddListener<GameLeaved>(OnGameLeaved);
        }

        private void OnDisable()
        {
            EventManager.UIBus.RemoveListener<StartGame>(OnStartGame);
            EventManager.UIBus.RemoveListener<BackToMenuButtonClicked>(OnBackToMenuButtonClicked);
            EventManager.UIBus.RemoveListener<GameLeaved>(OnGameLeaved);
        }

        #endregion

        #region BusCallbacks

        private void OnStartGame(StartGame pStartGame) 
        {
            DoEntranceWithDelay(_data.DelayBeforeEnter);
        }

        private void OnBackToMenuButtonClicked(BackToMenuButtonClicked pBackToMenuButtonClicked) 
        {
            DoEntranceWithDelay(_data.DelayBeforeEnter);
        }

        private void OnGameLeaved(GameLeaved pGameLeaved) 
        {
            DoEntranceWithDelay(_data.DelayBeforeEnter);
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

            EventManager.GameplayBus.RaiseEvent(new StartMatch());
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

        #endregion
    }
}

public class StartMatch : IEvent 
{
}