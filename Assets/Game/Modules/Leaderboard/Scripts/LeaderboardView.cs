using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using static ToolsACG.Services.DreamloLeaderboard.DreamloLeaderboardService;

namespace ToolsACG.Scenes.Leaderboard
{
    public interface ILeaderboardView
    {
        void TurnGeneralContainer(bool pState);

        void TurnLoadingSpinner(bool pState);
        void TurnErrorMessage(bool pState);
        void TurnRowsContainer(bool pState);
        void SetLeaderboardData(List<LeaderboardEntry> pData);
    }

    public class LeaderboardView : ModuleView, ILeaderboardView
    {
        #region Fields        

        [SerializeField] private GameObject _generalContainer;

        [SerializeField] private RectTransform _loadingSpinner;
        [SerializeField] private GameObject _errorMessage;
        [SerializeField] private GameObject _rowsContainer;
        [SerializeField] private List<LeaderboardRowHelper> _rowHelpers;

        #endregion

        #region Protected Methods     

        protected override void Awake()
        {
            base.Awake();
        }

        #endregion

        #region View Methods

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
        public void SetLeaderboardData(List<LeaderboardEntry> pData)
        {
            for (int i = 0; i < pData.Count; i++)
                _rowHelpers[i].SetData(i + 1, pData[i]);
        }

        #endregion

        #region Private Methods
        // TODO: define here methods called from view methods
        #endregion
    }
}