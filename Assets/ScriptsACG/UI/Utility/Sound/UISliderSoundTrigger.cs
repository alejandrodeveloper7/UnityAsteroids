using System.Collections.Generic;
using ToolsACG.Core.Managers;
using ToolsACG.Core.ScriptableObjects.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace ToolsACG.UI.Utilitys.Sound
{
    [RequireComponent(typeof(Slider))]

    public class UISliderSoundTrigger : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler
    {
        #region Fields 

        [Header("References")]
        [SerializeField] private List<SO_SoundData> _clickSounds;
        [SerializeField] private List<SO_SoundData> _hoverSounds;
        [Space]
        [Inject] private readonly ISoundManager _soundManager;

        [Header("States")]
        private bool _isInteracting;

        #endregion

        #region Interface Handlers

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_hoverSounds != null)
                _soundManager.Play2DSounds(_hoverSounds);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button is PointerEventData.InputButton.Left is false)
                return;

            _isInteracting = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_isInteracting is false)
                return;

            if (eventData.button is PointerEventData.InputButton.Left is false)
                return;

            _isInteracting = false;

            if (_clickSounds != null)
                _soundManager.Play2DSounds(_clickSounds);
        }

        #endregion
    }
}