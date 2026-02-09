using Asteroids.Core.Interfaces;
using Asteroids.Core.ScriptableObjects.Configurations;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Asteroids.UI.Controllers
{
    public class UISelectorController : MonoBehaviour
    {
        #region Fields, events and properties

        public event Action<int> SelectedElementUpdated;

        [Header("References")]
        [SerializeField] private Image _elementDisplayImage;

        [Header("Values")]
        private int _selectedIndex = 0;
        public int SelectedId { get; private set; }

        [Header("Data")]
        [Inject] private readonly SO_SelectorConfiguration _selectorConfiguration;
        [Space]
        private List<ISelectorElement> _availableItems;

        #endregion

        #region Initialization

        public void SetData(List<ISelectorElement> items, int initialIndex = 0)
        {
            _availableItems = items;
            _selectedIndex = initialIndex;

            UpdateSelectedItem(true, false);
        }

        #endregion

        #region Button callback

        public void PreviousElementButtonClick()
        {
            _selectedIndex--;
            UpdateSelectedItem(false, true);
        }

        public void NextElementButtonClick()
        {
            _selectedIndex++;
            UpdateSelectedItem(true, true);
        }

        #endregion

        #region Private Methods

        private void UpdateSelectedItem(bool isNext, bool useAnimation)
        {
            if (_availableItems == null || _availableItems.Count == 0)
                return;

            int validIndex;

            for (int i = 0; i < _availableItems.Count; i++)
            {
                validIndex = GetValidIndex(_selectedIndex);

                if (_availableItems[validIndex].IsActive)
                {
                    SelectedId = _availableItems[validIndex].Id;
                    SelectedElementUpdated?.Invoke(SelectedId);
                    UpdateDisplay(validIndex, useAnimation);
                    return;
                }

                _selectedIndex += isNext ? 1 : -1;
            }
        }

        private void UpdateDisplay(int index, bool useAnimation)
        {
            _elementDisplayImage.transform.DOKill();

            if (useAnimation)
            {
                Sequence seq = DOTween.Sequence();

                seq.Append(_elementDisplayImage.transform.DORotate(_selectorConfiguration.DisplayChangeRotationAngle, _selectorConfiguration.DisplayChangeDuration / 2));
                seq.AppendCallback(() => { _elementDisplayImage.sprite = _availableItems[index].Sprite; });
                seq.Append(_elementDisplayImage.transform.DORotate(Vector3.zero, _selectorConfiguration.DisplayChangeDuration / 2));
                seq.Play();
            }
            else
                _elementDisplayImage.sprite = _availableItems[index].Sprite;
        }

        private int GetValidIndex(int index)
        {
            return (index % _availableItems.Count + _availableItems.Count) % _availableItems.Count;
        }

        #endregion
    }
}