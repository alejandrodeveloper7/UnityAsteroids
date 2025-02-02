using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ToolsACG.Scenes.PlayerHealth
{
    public interface IPlayerHealthBarView
    {
        void TurnGeneralContainer(bool pState);

        void SetViewAlpha(float pValue);
        void ViewFadeTransition(float pDestinyValue, float pDuration);

        void SetMaxHealth(int pAmount);
        void SetCurrentHealth(int pAmount);
        void SetHealthPointsSprites(Sprite pHealthPointSprite, Sprite pEmptyHealthPointSprite);
        void SetShieldSliderSprites(Sprite pShieldBarSprite, Sprite pFullShieldBarSprite);
    }

    public class PlayerHealthBarView : ModuleView, IPlayerHealthBarView
    {
        #region Fields        

        [SerializeField] private GameObject _generalContainer;

        [Header("Healt Points")]
        [SerializeField] private GameObject _healthPointPrefab;
        [SerializeField] private Transform _healthPointsContainer;
        private List<Image> _currentHealtPoints= new List<Image>();
        [Space]
        private Sprite _healthPointSprite;
        private Sprite _emptyHealthPointSprite;

        [Header("Shield Bar")]
        [SerializeField] private Slider _shieldSlider;
        [SerializeField] private Image _shieldSliderImage;
        [Space]
        private Sprite _shieldBarSprite;
        private Sprite _fullShieldBarSprite;
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

        public void SetMaxHealth(int pAmount)
        {
            foreach (var item in _currentHealtPoints)
                Destroy(item.gameObject);

            _currentHealtPoints.Clear();

            for (int i = 0; i < pAmount; i++)
            {
                Image newHeathPoint = Instantiate(_healthPointPrefab, _healthPointsContainer).GetComponent<Image>();
                newHeathPoint.sprite = _healthPointSprite;
                _currentHealtPoints.Add(newHeathPoint);
            }
        }

        public void SetCurrentHealth(int pAmount)
        {
            for (int i = 0; i < _currentHealtPoints.Count; i++)
            {
                if (i < pAmount)
                    _currentHealtPoints[i].sprite = _healthPointSprite;
                else
                    _currentHealtPoints[i].sprite = _emptyHealthPointSprite;
            }
        }


        public void SetHealthPointsSprites(Sprite pHealthPointSprite, Sprite pEmptyHealthPointSprite)
        {
            _healthPointSprite = pHealthPointSprite;
            _emptyHealthPointSprite = pEmptyHealthPointSprite;
        }
        public void SetShieldSliderSprites(Sprite pShieldBarSprite, Sprite pFullShieldBarSprite)
        {
            _shieldBarSprite = pShieldBarSprite;
            _fullShieldBarSprite = pFullShieldBarSprite;
        }

        #endregion

        #region Private Methods
        // TODO: define here methods called from view methods
        #endregion
    }
}