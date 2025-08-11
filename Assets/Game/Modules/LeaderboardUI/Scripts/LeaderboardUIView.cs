using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static ToolsACG.ApiCaller.DreamloLeaderboardApiCaller.DreamloLeaderboardApiCaller;

namespace ToolsACG.Scenes.LeaderboardUI
{
    public class LeaderboardUIView : ModuleView
    {
        #region Fields     

        [Header("MVC References")]
        private LeaderboardUIController _controller;
        private LeaderboardUIModel _model;
        private SO_LeaderboardUIConfiguration _configuration;

        [Header("References")]
        [SerializeField] private GameObject _generalContainer;
        [Space]
        [SerializeField] private RectTransform _loadingSpinner;
        [SerializeField] private GameObject _errorMessage;
        [SerializeField] private GameObject _rowsContainer;
        [Space]
        [SerializeField] private List<LeaderboardRowController> _rowControllers;

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
            _controller = GetComponent<LeaderboardUIController>();
            _model = _controller.Model;
            _configuration = _controller.ModuleConfigurationData;
        }

        protected override void RegisterListeners()
        {
            // TODO: Create the listeners for the Model events
        }

        protected override void UnRegisterListeners()
        {
            // TODO: Erase the listeners for the Model events
        }

        #endregion

        #region Model Callbacks

        // TODO: Define here callbacks of the events from the model

        #endregion

        #region View Methods

        // TODO: Define here methods called from other view methods     

        #endregion

        #region Public Methods

        public void TurnGeneralContainer(bool pState)
        {
            _generalContainer.SetActive(pState);
        }

        public void TurnLoadingSpinner(bool pState)
        {
            if (pState)
            {
                _loadingSpinner.gameObject.SetActive(true);
                _loadingSpinner.transform
                    .DOLocalRotate(new Vector3(0, 0, -360), 1f, RotateMode.FastBeyond360)
                    .SetLoops(-1)
                    .SetEase(Ease.Linear);
            }
            else
            {
                _loadingSpinner.transform.DOKill();
                _loadingSpinner.gameObject.SetActive(false);
            }
        }

        public void TurnErrorMessage(bool pState)
        {
            _errorMessage.SetActive(pState);
        }

        public void TurnRowsContainer(bool pState)
        {
            _rowsContainer.SetActive(pState);
        }

        public void UpdateLeaderboardRowsData(List<LeaderboardEntry> pData)
        {
            for (int i = 0; i < pData.Count; i++)
                _rowControllers[i].SetData(i + 1, pData[i], _controller.ModuleConfigurationData.LeaderboardPlayerColor);

            TurnRowsContainer(true);
            TurnLoadingSpinner(false);
        }

        public void DisplayLeaderboardError() 
        {
            TurnErrorMessage(true);
            TurnRowsContainer(false);
            TurnLoadingSpinner(false);
        }

        public void RestartLeaderboardRows()
        {
            foreach (LeaderboardRowController row in _rowControllers)
                row.Restart();
        }

        public void RestartView()
        {
            TurnErrorMessage(false);
            TurnRowsContainer(false);
            TurnLoadingSpinner(true);
            RestartLeaderboardRows();
            SetViewAlpha(0);
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