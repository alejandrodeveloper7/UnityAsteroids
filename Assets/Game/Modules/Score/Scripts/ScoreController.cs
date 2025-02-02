using ToolsACG.Utils.Events;
using UnityEngine;

namespace ToolsACG.Scenes.Score
{
    [RequireComponent(typeof(ScoreView))]
    public class ScoreController : ModuleController
    {
        #region Private Fields

        private IScoreView _view;
        private ScoreModel _data;
        
        private int _score;
        private bool _alive;
        #endregion

        #region Properties

        public ScoreModel Model
        {
            get { return _data; }
            set { _data = value; }
        }

        #endregion

        #region Protected Methods

        protected override void Awake()
        {
            _view = GetComponent<IScoreView>();
            base.Awake();            
            _data = new ScoreModel();

            Initialize();
        }

        protected override void RegisterActions()
        {
            // TODO: initialize dictionaries with actions for buttons, toggles and dropdowns.      
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
            EventManager.GetGameplayBus().AddListener<StartMatch>(OnStartMatch);
            EventManager.GetGameplayBus().AddListener<AsteroidDestroyed>(OnAsteroidDestroyed);
            EventManager.GetGameplayBus().AddListener<PlayerDead>(OnPlayerDead);
            EventManager.GetUiBus().AddListener<BackToMenuButtonClicked>(OnBackToMenuButtonClicked);
        }

        private void OnDisable()
        {
            EventManager.GetGameplayBus().RemoveListener<StartMatch>(OnStartMatch);
            EventManager.GetGameplayBus().RemoveListener<AsteroidDestroyed>(OnAsteroidDestroyed);
            EventManager.GetUiBus().RemoveListener<BackToMenuButtonClicked>(OnBackToMenuButtonClicked);
        }

        #endregion

        #region Bus callbacks

        private void OnStartMatch(StartMatch pStartMatch) 
        {
            _alive = true;
            RestartScore();
            _view.SetViewAlpha(0);
            _view.TurnGeneralContainer(true);
            _view.ViewFadeTransition(1, 0.3f);
        }

        private void OnAsteroidDestroyed(AsteroidDestroyed pAsteroidDestroyed) 
        {
            if (_alive is false)
                return;

            _score += pAsteroidDestroyed.AsteroidData.PointsValue;
            _view.SetScore(_score);
        }

        private void OnPlayerDead(PlayerDead pPlayerDead) 
        {
            _alive = false;
            PersistentDataManager.LastScore = _score;
        }

        private void OnBackToMenuButtonClicked(BackToMenuButtonClicked pBackToMenuButtonClicked) 
        {
            _view.ViewFadeTransition(0, 0.3f);
        }

        #endregion

        private void Initialize() 
        {   
            _view.TurnGeneralContainer(false);
        }

        private void RestartScore() 
        {
            _score = 0;
            _view.SetScore(_score);
        }
    }
}