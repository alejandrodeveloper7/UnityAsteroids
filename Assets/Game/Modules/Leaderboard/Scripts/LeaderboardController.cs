using System;
using System.Threading.Tasks;
using ToolsACG.Services.DreamloLeaderboard;
using ToolsACG.Utils.Events;
using UnityEngine;

namespace ToolsACG.Scenes.Leaderboard
{
    [RequireComponent(typeof(LeaderboardView))]
    public class LeaderboardController : ModuleController
    {
        #region Private Fields

        private ILeaderboardView _view;
        [SerializeField] private LeaderboardModel _data;

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
            Actions.Add("BTN_BackToMenu", OnBackToMenuButtonClick);
        }

        protected override void UnRegisterActions()
        {
            // TODO: Unregister listeners and dictionarie actions.      
        }

        protected override void GetReferences()
        {
            _view = GetComponent<ILeaderboardView>();
        }

        protected override void Initialize()
        {
            _view.TurnGeneralContainer(false);
        }

        #endregion

        #region Monobehaviour

        private void OnEnable()
        {
            EventManager.GameplayBus.AddListener<PlayerDead>(OnPlayerDead);
        }

        private void OnDisable()
        {
            EventManager.GameplayBus.RemoveListener<PlayerDead>(OnPlayerDead);
        }

        #endregion

        #region Bus callbacks

        private void OnPlayerDead(PlayerDead pPlayerDead)
        {
            RestartView();
            DoEntranceWithDelay(_data.DelayBeforeEnter, SetScoreToAPI);
        }

        #endregion

        #region Button callbacks

        private void OnBackToMenuButtonClick()
        {
            EventManager.UIBus.RaiseEvent(new BackToMenuButtonClicked());
            DoExitWithDelay(0);
        }

        #endregion

        #region Apis Management

        private void SetScoreToAPI()
        {
            DreamloLeaderboardService.Instance.SetScore(PersistentDataManager.UserName, PersistentDataManager.LastScore
             ,
                  pResponse =>
                 {
                     if (pResponse.Contains("OK"))
                         GetScoresFromApi();
                     else
                     {
                         _view.TurnErrorMessage(true);
                         _view.TurnRowsContainer(true);
                     }
                 }
             ,
                  pResponse =>
                 {
                     _view.TurnErrorMessage(true);
                     _view.TurnRowsContainer(true);
                 }
             );
        }

        private void GetScoresFromApi()
        {
            DreamloLeaderboardService.Instance.GetRangeScores(_data.LeaderboardPositionsAmount
             ,
                 pResponse =>
                 {
                     _view.SetLeaderboardData(pResponse.Dreamlo.Leaderboard.Entry, _data.LeaderboardPlayerColor);
                     _view.TurnRowsContainer(true);
                     _view.TurnLoadingSpinner(false);
                 }
             ,
                 pResponse =>
                 {
                     _view.TurnErrorMessage(true);
                     _view.TurnRowsContainer(true);
                 }
             );
        }

        #endregion

        #region Navigation

        private void RestartView()
        {
            _view.TurnErrorMessage(false);
            _view.TurnRowsContainer(false);
            _view.TurnLoadingSpinner(true);
            _view.RestartLeaderboardRows();
            View.SetViewAlpha(0);
        }

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
    }
}

public class BackToMenuButtonClicked : IEvent
{ }