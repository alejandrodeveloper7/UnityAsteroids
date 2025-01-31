
using DG.Tweening;
using UnityEngine;

namespace ToolsACG.Scenes.Leaderboard
{
    public interface ILeaderboardView
    {
        void TurnGeneralContainer(bool pState);

        void SetViewAlpha(float pValue);
        void ViewFadeTransition(float pDestinyValue, float pDuration);
    }

    public class LeaderboardView : ModuleView, ILeaderboardView
    {
        #region Fields        

        [SerializeField] private GameObject _generalContainer;

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

        public void SetViewAlpha(float pValue)
        {
            CanvasGroup.alpha = pValue;
        }

        public void ViewFadeTransition(float pDestinyValue, float pDuration)
        {
            CanvasGroup.DOKill();
            CanvasGroup.DOFade(pDestinyValue, pDuration).SetEase(Ease.OutQuad);
        }

        #endregion

        #region Private Methods
        // TODO: define here methods called from view methods
        #endregion
    }
}