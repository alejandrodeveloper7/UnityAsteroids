using System;
using ToolsACG.ApiCaller.DreamloLeaderboardApiCaller;
using UnityEngine;
using UnityEngine.UI;

namespace ToolsACG.Scenes.LeaderboardUI
{
    [RequireComponent(typeof(LeaderboardUIView))]
    public class LeaderboardUIController : ModuleController
    {
        #region Private Fields

        [Header("View")]
        private LeaderboardUIView _view;

        [Header("Model")]
        private LeaderboardUIModel _model;
        public LeaderboardUIModel Model { get { return _model; } }

        [Header("Data")]
        [SerializeField] SO_LeaderboardUIConfiguration _configurationData;
        public SO_LeaderboardUIConfiguration ModuleConfigurationData { get { return _configurationData; } }

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
            EventManager.GameplayBus.AddListener<PlayerDead>(OnPlayerDead);
            Actions["BTN_BackToMenu"] = (Action<Button>)(btn => OnBackToMenuButtonClick(btn));
        }

        protected override void UnRegisterActions()
        {
            EventManager.GameplayBus.RemoveListener<PlayerDead>(OnPlayerDead);
            Actions.Clear();
        }

        protected override void GetReferences()
        {
            _view = GetComponent<LeaderboardUIView>();
        }

        protected override void Initialize()
        {
            _view.TurnGeneralContainer(false);
        }

        protected override void CreateModel()
        {
            _model = new LeaderboardUIModel();
        }

        #endregion

        #region UI Elements Actions callbacks

        private void OnBackToMenuButtonClick(Button pButton)
        {
            EventManager.UIBus.RaiseEvent(new BackToMenuButtonClicked());
            _view.ExitTransion(_configurationData.DelayBeforeExit, _configurationData.FadeTransitionDuration, null);
        }

        #endregion

        #region Bus callbacks

        private void OnPlayerDead(PlayerDead pPlayerDead)
        {
            _view.RestartView();
            _view.EnterTransition(_configurationData.DelayBeforeEnter, _configurationData.FadeTransitionDuration, SetScoreToAPI);
        }

        #endregion

        #region Apis Management

        private async void SetScoreToAPI()
        {
            bool sucess = await DreamloLeaderboardApiCaller.Instance.SetScore(PersistentDataManager.UserName, PersistentDataManager.LastScore);

            if (sucess)
                GetScoresFromApi();
            else
                _view.DisplayLeaderboardError();
        }

        private async void GetScoresFromApi()
        {
            bool sucess = await DreamloLeaderboardApiCaller.Instance.GetRangeScores(_configurationData.LeaderboardPositionsAmount);

            if (sucess)
                _view.UpdateLeaderboardRowsData(DreamloLeaderboardApiCaller.Instance.RangeScoreResponse.Dreamlo.Leaderboard.Entry);
            else
                _view.DisplayLeaderboardError();
        }

        #endregion
    }
}

public readonly struct BackToMenuButtonClicked : IEvent
{ }