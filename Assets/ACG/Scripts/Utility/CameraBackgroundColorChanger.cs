using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace ACG.Scripts.Utilitys
{
    public class CameraBackgroundColorChanger : MonoBehaviour
    {
        #region Fields

        [Header("References")]
        [SerializeField] private Camera _camera;

        [Header("Settings")]
        [SerializeField] private List<Color> _colors;
        [SerializeField] private float _changeInterval = 6f;
        [SerializeField] private float _changeDuration = 2f;

        [Header("Data")]
        private int _currentColorIndex = 0;

        #endregion

        #region Monobehaviour

        private void Start()
        {
            if (_camera == null)
                _camera = Camera.main;

            if (_camera == null)
            {
                Debug.LogWarning($"{nameof(CameraBackgroundColorChanger)} on {gameObject.name} has no camera set. Component disabled");
                enabled = false;
                return;
            }

            if (_colors is null || _colors.Count is 0)
            {
                Debug.LogWarning($"{nameof(CameraBackgroundColorChanger)} on {gameObject.name} has no colors set. Component disabled");
                enabled = false;
                return;
            }

            _currentColorIndex = Random.Range(0, _colors.Count);
            _camera.backgroundColor = _colors[_currentColorIndex];

            InvokeRepeating(nameof(ChangeColor), _changeInterval, _changeInterval);
        }

        private void OnDestroy()
        {
            CancelInvoke(nameof(ChangeColor));
            _camera.DOKill();
        }

        #endregion

        #region Functionality

        private void ChangeColor()
        {
            if (_colors.Count <= 1)
                return;

            int nextIndex;
            
            do
            {
                nextIndex = Random.Range(0, _colors.Count);
            }
            while (nextIndex == _currentColorIndex);

            _currentColorIndex = nextIndex;
            _camera.DOColor(_colors[_currentColorIndex], _changeDuration);
        }

        #endregion
    }
}