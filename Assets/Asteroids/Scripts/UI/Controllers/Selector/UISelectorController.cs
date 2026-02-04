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

        [Header("Data")]
        [Inject] private readonly SO_SelectorConfiguration _selectorConfiguration;
        [Space]
        private List<ISelectorElement> _availableItems;

        [Header("Values")]
        private int _selectedIndex = 0;
        public int SelectedId { get; private set; }

        #endregion

        #region Initialization

        public void SetData(List<ISelectorElement> items, int initialIndex = 0)
        {
            _availableItems = items;
            _selectedIndex = initialIndex;

            UpdateSelectedItem();
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

        private void UpdateSelectedItem(bool isNext = true, bool useAnimation = false)
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
                    _ = UpdateDisplay(validIndex, useAnimation);
                    return;
                }

                _selectedIndex += isNext ? 1 : -1;
            }
        }

        private async Task UpdateDisplay(int index, bool useAnimation = false)
        {
            _elementDisplayImage.transform.DOKill();

            if (useAnimation)
            {
                await _elementDisplayImage.transform.DORotate(_selectorConfiguration.DisplayedItemAnimationRotationAngle, _selectorConfiguration.DisplayedItemChangeAnimationTotalDuration * 0.5f).AsyncWaitForCompletion();
                _elementDisplayImage.sprite = _availableItems[index].Sprite;
                await _elementDisplayImage.transform.DORotate(new Vector3(0, 0, 0), _selectorConfiguration.DisplayedItemChangeAnimationTotalDuration * 0.5f).AsyncWaitForCompletion();
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