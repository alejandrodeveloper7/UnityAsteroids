using ACG.Scripts.Models;
using System.Collections;
using UnityEngine;

namespace ACG.Scripts.Utilitys
{
    public class ScreenEdgeTeleport : MonoBehaviour
    {
        #region Fields

        [Header("Gameplay References")]
        private Camera _camera;

        [Header("Cache")]
        private Coroutine _currentRelocationCoroutine;

        [Header("Data")]
        private ScreenEdgeTeleportConfiguration _configuration;

        #endregion

        #region initialization

        public void SetData(ScreenEdgeTeleportConfiguration config)
        {
            _configuration = config;
            StartRelocationCoroutine();
        }

        #endregion

        #region Monobehaviour

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void OnDisable()
        {
            StopRelocationCoroutine();
        }

        #endregion

        #region Functionality

        private void StartRelocationCoroutine()
        {
            if (_currentRelocationCoroutine != null)
                StopCoroutine(_currentRelocationCoroutine);

            _currentRelocationCoroutine = StartCoroutine(RelocationCoroutine());
        }

        private void StopRelocationCoroutine()
        {
            if (_currentRelocationCoroutine != null)
            {
                StopCoroutine(_currentRelocationCoroutine);
                _currentRelocationCoroutine = null;
            }
        }

        private IEnumerator RelocationCoroutine()
        {
            while (true)
            {
                Vector3 viewportPosition = _camera.WorldToViewportPoint(transform.position);
                Vector3 newPosition = transform.position;

                if (viewportPosition.x > 1 + _configuration.EdgeOffsetX)
                {
                    newPosition.x = _camera.ViewportToWorldPoint(new Vector2(-_configuration.EdgeRepositionOffsetX, viewportPosition.y)).x;
                    transform.position = newPosition;
                }
                else if (viewportPosition.x < 0 - _configuration.EdgeOffsetX)
                {
                    newPosition.x = _camera.ViewportToWorldPoint(new Vector2(1 + _configuration.EdgeRepositionOffsetX, viewportPosition.y)).x;
                    transform.position = newPosition;
                }
                else if (viewportPosition.y > 1 + _configuration.EdgeOffsetY)
                {
                    newPosition.y = _camera.ViewportToWorldPoint(new Vector2(viewportPosition.x, -_configuration.EdgeRepositionOffsetY)).y;
                    transform.position = newPosition;
                }
                else if (viewportPosition.y < 0 - _configuration.EdgeOffsetY)
                {
                    newPosition.y = _camera.ViewportToWorldPoint(new Vector2(viewportPosition.x, 1 + _configuration.EdgeRepositionOffsetY)).y;
                    transform.position = newPosition;
                }

                yield return new WaitForSeconds(_configuration.CheckInterval);
            }
        }

        #endregion
    }
}