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
            _view = GetComponent<ILeaderboardView>();
            base.Awake();

            Initialize();
        }

        protected override void RegisterActions()
        {
            Actions.Add("BTN_BackToMenu", OnBackToMenuButtonClick);
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
            EventManager.GetGameplayBus().AddListener<PlayerDead>(OnPlayerDead);
        }

        private void OnDisable()
        {
            EventManager.GetGameplayBus().RemoveListener<PlayerDead>(OnPlayerDead);
        }

        #endregion

        #region Bus callbacks

        private async void OnPlayerDead(PlayerDead pPlayerDead)
        {
            RestartView();

            await Task.Delay(2000);
            _view.TurnGeneralContainer(true);
            View.DoFadeTransition(1, 0.3f);
            await Task.Delay(300);

            SetScore();

        }

        #endregion

        #region Apis Management

        private void SetScore()
        {
            DreamloLeaderboardService.Instance.SetScore(PersistentDataManager.UserName, PersistentDataManager.LastScore
             ,
                  pResponse =>
                 {
                     if (pResponse.Contains("OK"))
                         GetScores();
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

        private void GetScores()
        {
            DreamloLeaderboardService.Instance.GetRangeScores(8
             ,
                 pResponse =>
                 {
                     _view.SetLeaderboardData(pResponse.Dreamlo.Leaderboard.Entry);
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

        #region Button callbacks

        private async void OnBackToMenuButtonClick()
        {
            View.DoFadeTransition(0, 0.3f);
            _=EventManager.GetUiBus().RaiseEvent(new BackToMenuButtonClicked());
            await Task.Delay(300);
            _view.TurnGeneralContainer(false);
        }

        #endregion

        private void Initialize()
        {
            _view.TurnGeneralContainer(false);
        }

        private void RestartView()
        {
            _view.TurnErrorMessage(false);
            _view.TurnRowsContainer(false);
            _view.TurnLoadingSpinner(true);
            View.SetViewAlpha(0);
        }

    }
}

public class BackToMenuButtonClicked : IEvent
{ }