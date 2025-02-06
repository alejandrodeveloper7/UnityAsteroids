using System;
using System.Threading.Tasks;
using ToolsACG.Utils.Events;
using UnityEngine;

namespace ToolsACG.Scenes.Score
{
    [RequireComponent(typeof(ScoreView))]
    public class ScoreController : ModuleController
    {
        #region Private Fields

        private IScoreView _view;
        [SerializeField] private ScoreModel _data;
        [Space]
        private int _score;
        private bool _alive;

        #endregion

        #region Protected Methods

        protected override void Awake()
        {
            _view = GetComponent<IScoreView>();
            base.Awake();
        }

        protected override void RegisterActions()
        {
            // TODO: initialize dictionaries with actions for buttons, toggles, sliders, input fields and dropdowns.      
        }

        protected override void Initialize()
        {
            _view.TurnGeneralContainer(false);
        }

        #endregion

        #region Monobehaviour

        private void OnEnable()
        {
            EventManager.GetGameplayBus().AddListener<StartMatch>(OnStartMatch);
            EventManager.GetGameplayBus().AddListener<AsteroidDestroyed>(OnAsteroidDestroyed);
            EventManager.GetGameplayBus().AddListener<PlayerDead>(OnPlayerDead);
            EventManager.GetUiBus().AddListener<BackToMenuButtonClicked>(OnBackToMenuButtonClicked);
            EventManager.GetUiBus().AddListener<GameLeaved>(OnGameLeaved);
        }

        private void OnDisable()
        {
            EventManager.GetGameplayBus().RemoveListener<StartMatch>(OnStartMatch);
            EventManager.GetGameplayBus().RemoveListener<AsteroidDestroyed>(OnAsteroidDestroyed);
            EventManager.GetGameplayBus().RemoveListener<PlayerDead>(OnPlayerDead);
            EventManager.GetUiBus().RemoveListener<BackToMenuButtonClicked>(OnBackToMenuButtonClicked);
            EventManager.GetUiBus().RemoveListener<GameLeaved>(OnGameLeaved);
        }

        #endregion

        #region Bus callbacks

        private void OnStartMatch(StartMatch pStartMatch)
        {
            _alive = true;
            RestartScore();
            DoEntranceWithDelay(0);
        }

        private void OnAsteroidDestroyed(AsteroidDestroyed pAsteroidDestroyed)
        {
            if (_alive is false)
                return;

            AddScore(pAsteroidDestroyed.AsteroidData.PointsValue);
        }

        private void OnPlayerDead(PlayerDead pPlayerDead)
        {
            _alive = false;
            PersistentDataManager.LastScore = _score;
        }

        private void OnBackToMenuButtonClicked(BackToMenuButtonClicked pBackToMenuButtonClicked)
        {
            DoExitWithDelay(0);
        }

        private void OnGameLeaved(GameLeaved pGameLeaved)
        {
            _alive = false;
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

        private void RestartScore()
        {
            _score = 0;
            _view.SetScore(_score);
        }

        private void AddScore(int pValue)
        {
            _score += pValue;
            _view.SetScore(_score);
        }

        #endregion
    }
}