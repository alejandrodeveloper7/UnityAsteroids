using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ToolsACG.UI.Utilitys.EventHandlers
{
    [RequireComponent(typeof(Slider))]

    public class UISliderEventsHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        #region Fields and events

        public Action<float> OnEndDrag { get; set; }

        [Header("References")]
        [SerializeField] private Slider _slider;

        [Header("States")]
        private bool _isInteracting;

        #endregion

        #region Interface Handlers

        public void OnPointerDown(PointerEventData eventData)
        {
            _isInteracting = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_isInteracting is false)
                return;

            _isInteracting = false;
            OnEndDrag?.Invoke(_slider.value);
        }

        #endregion
    }
}