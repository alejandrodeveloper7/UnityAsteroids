using DG.Tweening;
using UnityEngine;

namespace ToolsACG.Scenes
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class ModuleView : MonoBehaviour
    {
        #region Fields

        protected CanvasGroup CanvasGroup;

        #endregion

        #region Protected Methods
        
        protected virtual void Awake()
        {
            CanvasGroup = GetComponent<CanvasGroup>();
        }

        #endregion

        #region Abstract Methods

        protected abstract void RegisterListeners();
        protected abstract void UnRegisterListeners();
        protected abstract void GetReferences();

        #endregion

        #region Public Methods

        public void SetViewAlpha(float pValue)
        {
            CanvasGroup.alpha = pValue;
            SetCanvasGroupDetection(pValue == 1);
        }

        public void DoFadeTransition(float pDestinyValue, float pDuration, Ease pEase=Ease.OutQuad)
        {
            CanvasGroup.DOKill();
            SetCanvasGroupDetection(false);

            CanvasGroup.DOFade(pDestinyValue, pDuration)
                .SetEase(pEase)
                .OnComplete
                (
                 () => { SetCanvasGroupDetection(pDestinyValue == 1); }
                );
        }

        public void SetCanvasGroupDetection(bool pState)
        {
            CanvasGroup.blocksRaycasts = pState;
            CanvasGroup.interactable = pState;
        }

        #endregion
    }
}