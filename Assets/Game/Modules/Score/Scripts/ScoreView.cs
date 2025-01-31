
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace ToolsACG.Scenes.Score
{
    public interface IScoreView
    {
        void TurnGeneralContainer(bool pState);

        void SetViewAlpha(float pValue);
        void ViewFadeTransition(float pDestinyValue, float pDuration);

        void SetScore(int pValue);
    }

    public class ScoreView : ModuleView, IScoreView
    {
        #region Fields  

        [SerializeField] private GameObject _generalContainer;
        [SerializeField] private TextMeshProUGUI _scoreText;
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

        public void SetScore(int pvalue) 
        {
        _scoreText.text=pvalue.ToString();
        }


        #endregion

        #region Private Methods
        // TODO: define here methods called from view methods
        #endregion
    }
}