using ACG.Tools.Runtime.MVCModulesCreator.Bases;
using Asteroids.MVC.ScoreUI.Controllers;
using Asteroids.MVC.ScoreUI.Models;
using Asteroids.MVC.ScoreUI.ScriptableObjects;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;

namespace Asteroids.MVC.ScoreUI.Views
{
    public class ScoreUIView : MVCViewBase, IScoreUIView
    {
        #region Fields     

        [Header("MVC References")]
        [Inject] private readonly IScoreUIController _controller;
        [Inject] private readonly ScoreUIModel _model;
        [Inject] private readonly SO_ScoreUIConfiguration _configuration;

        [Header("References")]
        [SerializeField] private Transform _scorePanel;
        [SerializeField] private TextMeshProUGUI _scoreText;

        [Header("Values")]
        private int _displayingScore;

        [Header("Cache")]
        private Tween _currentTween;

        #endregion

        #region Initialization

        protected override void GetReferences()
        {
            base.GetReferences();
        }

        protected override void Initialize()
        {
            base.Initialize();

            SetGeneralContainerActive(_configuration.ViewInitialState);
            SetGeneralContainerScale(_configuration.ViewInitialScale);
            SetAlpha(_configuration.ViewInitialAlpha);
        }

        protected override void RegisterListeners()
        {
            _model.ScoreUpdated += OnScoreUpdated;
            _model.ScoreRestarted += OnScoreRestarted;
        }

        protected override void UnRegisterListeners()
        {
            _model.ScoreUpdated -= OnScoreUpdated;
            _model.ScoreRestarted -= OnScoreRestarted;
        }

        #endregion

        #region Monobehaviour        

        protected override void Awake()
        {
            base.Awake();

            GetReferences();
            RegisterListeners();
        }

        protected override void Start()
        {
            base.Start();

            Initialize();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            UnRegisterListeners();
        }

        #endregion

        #region Event Callbacks

        private void OnScoreUpdated(int score)
        {
            SetScore(score,true);
            PlayScoreFeedbackTween();
        }

        private void OnScoreRestarted()
        {
            SetScore(0,false);
        }

        #endregion

        #region public Methods

        // TODO: Define here methods called from the controller     

        #endregion

        #region View Methods               

        private void PlayScoreFeedbackTween()
        {
            _scorePanel.DOKill();
            _scorePanel.localScale = Vector3.one;

            _scorePanel.DOScale(_configuration.ScorePanelFeedbackScale, _configuration.ScorePanelFeedbackDuration * 0.5f)
                .SetEase(Ease.OutBack).SetLoops(2, LoopType.Yoyo);
        }

        private void SetScore(int value, bool progressively)
        {
            _currentTween?.Kill();

            if (progressively)
            {
                int current = _displayingScore;
                int to = value;

                _currentTween = DOTween.To(() => current, x =>
                {
                    current = x;
                    _displayingScore = x;
                    _scoreText.text = current.ToString("N0");
                }
                , to, _configuration.ScoreIncreaseDuration).SetEase(Ease.OutCubic);
            }
            else
            {
                _scoreText.text = value.ToString("N0");
                _displayingScore = value;
            }
        }

        #endregion
    }
}