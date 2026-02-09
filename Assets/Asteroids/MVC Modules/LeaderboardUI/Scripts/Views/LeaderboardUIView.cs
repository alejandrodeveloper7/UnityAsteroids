using ACG.Tools.Runtime.MVCModulesCreator.Bases;
using Asteroids.ApiCallers.DreamloLeaderboardApiCaller;
using Asteroids.MVC.LeaderboardUI.Controllers;
using Asteroids.MVC.LeaderboardUI.Models;
using Asteroids.MVC.LeaderboardUI.ScriptableObjects;
using Asteroids.UI.Controllers;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Asteroids.MVC.LeaderboardUI.Views
{
    public class LeaderboardUIView : MVCViewBase, ILeaderboardUIView
    {
        #region Fields     

        [Header("MVC References")]
        [Inject] private readonly ILeaderboardUIController _controller;
        [Inject] private readonly LeaderboardUIModel _model;
        [Inject] private readonly SO_LeaderboardUIConfiguration _configuration;

        [Header("References")]
        [SerializeField] private RectTransform _loadingSpinner;
        [SerializeField] private GameObject _errorMessage;
        [SerializeField] private GameObject _rowsContainer;
        [Space]
        [SerializeField] private List<LeaderboardRowController> _rowControllers;

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
            _model.LeaderboardDataUpdated += OnLeaderboardDataUpdated;
        }

        protected override void UnRegisterListeners()
        {
            _model.LeaderboardDataUpdated -= OnLeaderboardDataUpdated;
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

        private void OnLeaderboardDataUpdated(Leaderboard data)
        {
            UpdateLeaderboardRowsData(data.Entry, _model.Username);
        }

        #endregion

        #region Public Methods

        public void RestartView()
        {
            SetErrorMessageActive(false);
            SetRowsContainerActive(false);
            SetLoadingSpinnerActive(true);
        }

        public void DisplayLeaderboardError()
        {
            SetErrorMessageActive(true);
            SetRowsContainerActive(false);
            SetLoadingSpinnerActive(false);
        }

        public void UpdateLeaderboardRowsData(List<LeaderboardEntry> data, string playerUserName)
        {
            for (int i = 0; i < data.Count; i++)
            {
                bool isPlayerScore = playerUserName == data[i].Name;
                _rowControllers[i].SetData(i + 1, data[i], isPlayerScore);
            }

            SetRowsContainerActive(true);
            SetLoadingSpinnerActive(false);
        }

        #endregion

        #region View Methods

        private void SetLoadingSpinnerActive(bool state)
        {
            if (state)
            {
                _loadingSpinner.gameObject.SetActive(true);
                _loadingSpinner.transform
                   .DOLocalRotate(new Vector3(0, 0, -360), 1f, RotateMode.LocalAxisAdd)
                   .SetEase(Ease.Linear)
                   .SetLoops(-1);
            }
            else
            {
                _loadingSpinner.transform.DOKill();
                _loadingSpinner.gameObject.SetActive(false);
            }
        }

        private void SetErrorMessageActive(bool state)
        {
            _errorMessage.SetActive(state);
        }

        private void SetRowsContainerActive(bool state)
        {
            _rowsContainer.SetActive(state);
        }

        #endregion
    }
}