using ACG.Scripts.Managers;
using ACG.Scripts.ScriptableObjects.Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace ACG.Scripts.UIUtilitys.Sound
{
    public class UISoundTrigger : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
    {
        #region Fields

        [Header("References")]
        [SerializeField] private List<SO_SoundData> _clickSounds;
        [SerializeField] private List<SO_SoundData> _hoverSounds;
        [Space]
        [Inject] private readonly ISoundManager _soundManager;

        #endregion

        #region Interface Handlers

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button is PointerEventData.InputButton.Left is false)
                return;

            if (_clickSounds != null)
                _soundManager.Play2DSound(_clickSounds);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_hoverSounds != null)
                _soundManager.Play2DSound(_hoverSounds);
        }

        #endregion
    }
}