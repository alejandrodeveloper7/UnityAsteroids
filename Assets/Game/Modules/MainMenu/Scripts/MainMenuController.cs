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
            _view = GetComponent<IMainMenuView>();
            base.Awake();
        }

        protected override void RegisterActions()
        {
            Actions.Add("BTN_Play", OnPlayButtonClick);
            Actions.Add("BTN_ExitGame", OnExitGameButtonClick);
        }

        protected override void Initialize()
        {
            _bulletSelectorController.SetData(ResourcesManager.Instance.BulletsConfiguration.Bullets.Cast<SO_Selectable>().ToList(), 0);
            _shipSelectorController.SetData(ResourcesManager.Instance.ShipsConfiguration.Ships.Cast<SO_Selectable>().ToList(), 0);

            _view.TurnGeneralContainer(false);
            _view.SetUserNameValue(Environment.UserName);
        }

        #endregion

        #region Monobehaviour

        private void OnEnable()
        {
            EventManager.GetUiBus().AddListener<StartGame>(OnStartGame);
            EventManager.GetUiBus().AddListener<BackToMenuButtonClicked>(OnBackToMenuButtonClicked);
            EventManager.GetUiBus().AddListener<GameLeaved>(OnGameLeaved);
        }

        private void OnDisable()
        {
            EventManager.GetUiBus().RemoveListener<StartGame>(OnStartGame);
            EventManager.GetUiBus().RemoveListener<BackToMenuButtonClicked>(OnBackToMenuButtonClicked);
            EventManager.GetUiBus().RemoveListener<GameLeaved>(OnGameLeaved);
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

            EventManager.GetGameplayBus().RaiseEvent(new StartMatch());
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