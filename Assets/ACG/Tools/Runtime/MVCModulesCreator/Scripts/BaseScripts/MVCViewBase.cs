using ACG.Core.Utils;
using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine;

namespace ACG.Tools.Runtime.MVCModulesCreator.Bases
{
    [RequireComponent(typeof(CanvasGroup))]

    public abstract class MVCViewBase : MonoBehaviour
    {
        #region Fields

        [Header("References")]
        [SerializeField] protected GameObject GeneralContainer;
        protected CanvasGroup CanvasGroup;

        #endregion

        #region Initialization

        protected virtual void GetReferences()
        {
            CanvasGroup = GetComponent<CanvasGroup>();
        }
        protected virtual void Initialize() { }
        protected abstract void RegisterListeners();
        protected abstract void UnRegisterListeners();

        #endregion

        #region Monobehaviour

        protected virtual void Awake() { }
        protected virtual void Start() { }
        protected virtual void OnDestroy() { }

        #endregion

        #region Scale Methods

        public void SetGeneralContainerActive(bool state)
        {
            GeneralContainer.SetActive(state);
        }

        public void SetGeneralContainerScale(Vector3 value)
        {
            GeneralContainer.transform.DOKill();
            GeneralContainer.transform.localScale = value;
        }

        public async Task PlayScaleTransition(float destinyScale, float duration, Ease ease = Ease.OutQuad)
        {
            GeneralContainer.transform.DOKill();

            await GeneralContainer.transform.DOScale(Vector3.one * destinyScale, duration)
                .SetEase(ease)
                .SetUpdate(true)
                .AsyncWaitForCompletion();
        }

        #endregion

        #region Canvas Group Methods

        public void SetAlpha(float value)
        {
            CanvasGroup.alpha = value;
        }

        public async Task PlayAlphaTransition(float destinyValue, float duration, Ease ease = Ease.OutQuad)
        {
            CanvasGroup.DOKill();

            await CanvasGroup.DOFade(destinyValue, duration)
                .SetEase(ease)
                .SetUpdate(true)
                .AsyncWaitForCompletion();
        }
        
        public void SetCanvasGroupDetection(bool state)
        {
            CanvasGroup.blocksRaycasts = state;
            CanvasGroup.interactable = state;
        }

        #endregion

        #region Animation

        public virtual async Task PlayEnterTransition(float delay, float transitionDuration)
        {
            await TimingUtils.WaitSeconds(delay, true);
            SetGeneralContainerActive(true);
            Task scaleTransition = PlayScaleTransition(1, transitionDuration);
            Task alphaTransition = PlayAlphaTransition(1, transitionDuration);
            await Task.WhenAll(scaleTransition, alphaTransition);
            SetCanvasGroupDetection(true);
        }

        public virtual async Task PlayExitTransition(float delay, float transitionDuration)
        {
            await TimingUtils.WaitSeconds(delay, true);
            SetCanvasGroupDetection(false);
            Task scaleTransition = PlayScaleTransition(0, transitionDuration);
            Task alphaTransition = PlayAlphaTransition(0, transitionDuration);
            await Task.WhenAll(scaleTransition, alphaTransition);
            SetGeneralContainerActive(false);
        }

        public void CleanTweens() 
        {
            CanvasGroup.DOKill();
            GeneralContainer.transform.DOKill();
        }

        #endregion
    }
}