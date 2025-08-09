using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using static ToolsACG.ApiCaller.DreamloLeaderboardApiCaller.DreamloLeaderboardApiCaller;

namespace ToolsACG.Scenes.Leaderboard
{
    public interface ILeaderboardView
    {
        void TurnGeneralContainer(bool pState);

        void TurnLoadingSpinner(bool pState);
        void TurnErrorMessage(bool pState);
        void TurnRowsContainer(bool pState);

        void SetLeaderboardData(List<LeaderboardEntry> pData, Color pPlayerColor);
        void RestartLeaderboardRows();
    }

    public class LeaderboardView : ModuleView, ILeaderboardView
    {
        #region Fields        

        [SerializeField] private GameObject _generalContainer;
        [Space]
        [SerializeField] private RectTransform _loadingSpinner;
        [SerializeField] private GameObject _errorMessage;
        [SerializeField] private GameObject _rowsContainer;
        [Space]
        [SerializeField] private List<LeaderboardRowHelper> _rowHelpers;

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

        public void SetLeaderboardData(List<LeaderboardEntry> pData, Color pPlayerColor)
        {
            for (int i = 0; i < pData.Count; i++)
                _rowHelpers[i].SetData(i + 1, pData[i], pPlayerColor);
        }
        public void RestartLeaderboardRows()
        {
            foreach (LeaderboardRowHelper row in _rowHelpers)
                row.Restart();
        }

        #endregion
    }
}