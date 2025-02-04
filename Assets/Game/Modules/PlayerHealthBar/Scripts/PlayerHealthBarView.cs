using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ToolsACG.Scenes.PlayerHealth
{
    public interface IPlayerHealthBarView
    {
        void TurnGeneralContainer(bool pState);
    
        void SetMaxHealth(int pAmount);
        void SetCurrentHealth(int pAmount);
        void SetHealthPointsSprites(Sprite pHealthPointSprite, Sprite pEmptyHealthPointSprite);
        void SetShieldSliderSprites(Sprite pShieldBarSprite, Sprite pFullShieldBarSprite);

        void SetShieldSliderValue(float pValue);
        void DoShielSliderTransition(float pValue, float pDuration);
    }

    public class PlayerHealthBarView : ModuleView, IPlayerHealthBarView
    {
        #region Fields        

        [SerializeField] private GameObject _generalContainer;

        [Header("Healt Points")]
        [SerializeField] private GameObject _healthPointPrefab;
        [SerializeField] private Transform _healthPointsContainer;
        private List<Image> _currentHealtPoints = new List<Image>();
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

        public void SetShieldSliderValue(float pValue)
        {
            _shieldSlider.value = pValue;
            UpdateSliderSprite();
        }

        public void DoShielSliderTransition(float pValue, float pDuration)
        {
            _shieldSlider.DOValue(pValue, pDuration);
            UpdateSliderSprite();
        }

        #endregion

        #region Private Methods
        private void UpdateSliderSprite()
        {
            if (_shieldSlider.value == _shieldSlider.maxValue)
                _shieldSliderImage.sprite = _fullShieldBarSprite;
            else
                _shieldSliderImage.sprite = _shieldBarSprite;
        }
        #endregion
    }
}