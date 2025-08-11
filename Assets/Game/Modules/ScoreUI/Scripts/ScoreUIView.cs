using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace ToolsACG.Scenes.ScoreUI
{
    public class ScoreUIView : ModuleView
    {
        #region Fields     

        [Header("MVC References")]
        private ScoreUIController _controller;
        private ScoreUIModel _model;
        private SO_ScoreUIConfiguration _configuration;

        [Header("References")]
        [SerializeField] private GameObject _generalContainer;
        [SerializeField] private TextMeshProUGUI _scoreText;

        #endregion

        #region Monobehaviour        

        protected override void Awake()
        {
            base.Awake();

            GetReferences();
            RegisterListeners();
        }

        private void OnDestroy()
        {
            UnRegisterListeners();
        }

        #endregion

        #region Initialization

        protected override void GetReferences()
        {
            _controller = GetComponent<ScoreUIController>();
            _model = _controller.Model;
            _configuration = _controller.ModuleConfigurationData;
        }

        protected override void RegisterListeners()
        {
            _model.OnScoreUpdated += OnScoreUpdated;
        }

        protected override void UnRegisterListeners()
        {
            _model.OnScoreUpdated -= OnScoreUpdated;
        }

        #endregion

        #region Model Callbacks

        private void OnScoreUpdated(int pScore)
        {
            SetScore(pScore);
        }

        #endregion

        #region View Methods               

        private void SetScore(int pvalue)
        {
            _scoreText.text = pvalue.ToString();
        }
       
        #endregion

        #region Public Methods

        public void TurnGeneralContainer(bool pState)
        {
            _generalContainer.SetActive(pState);
        }

        public async void EnterTransition(float pDelay, float pTransitionDuration, Action pOnComplete = null)
        {
            await Task.Delay((int)(pDelay * 1000));
            SetViewAlpha(0);
            TurnGeneralContainer(true);
            DoFadeTransition(1, pTransitionDuration);
            await Task.Delay((int)(pTransitionDuration * 1000));
            pOnComplete?.Invoke();
        }

        public async void ExitTransion(float pDelay, float pTransitionDuration, Action pOnComplete = null)
        {
            await Task.Delay((int)(pDelay * 1000));
            SetViewAlpha(1);
            DoFadeTransition(0, pTransitionDuration);
            await Task.Delay((int)(pTransitionDuration * 1000));
            TurnGeneralContainer(false);
            pOnComplete?.Invoke();
        }

        #endregion
    }
}