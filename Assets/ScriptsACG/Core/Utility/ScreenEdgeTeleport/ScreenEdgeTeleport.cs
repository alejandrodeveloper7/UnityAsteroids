using System.Collections;
using UnityEngine;

namespace ToolsACG.Core.Utilitys
{
    public class ScreenEdgeTeleport : MonoBehaviour
    {
        #region Fields

        [Header("References")]
        private Camera _camera;

        [Header("Configuration")]
        private float _edgeOffsetY = 0.05f;
        private float _edgeRepositionOffsetY = 0.04f;
        [Space]
        private float _edgeOffsetX = 0.04f;
        private float _edgeRepositionOffsetX = 0.03f;
        [Space]
        private float _checkInterval = 0.05f;

        [Header("Cache")]
        private Coroutine _currentRelocationCoroutine;

        #endregion

        #region initialization

        private void GetReferences()
        {
            _camera = Camera.main;
        }

        public void SetData(ScreenEdgeTeleportConfiguration config)
        {
            _edgeOffsetY = config.EdgeOffsetY;
            _edgeRepositionOffsetY = config.EdgeRepositionOffsetY;

            _edgeOffsetX = config.EdgeOffsetX;
            _edgeRepositionOffsetX = config.EdgeRepositionOffsetX;

            _checkInterval = config.CheckInterval;
        }

        #endregion

        #region Monobehaviour

        private void Awake()
        {
            GetReferences();
        }

        private void OnEnable()
        {
            StartRelocationCoroutine();
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

                if (viewportPosition.x > 1 + _edgeOffsetX)
                {
                    newPosition.x = _camera.ViewportToWorldPoint(new Vector2(-_edgeRepositionOffsetX, viewportPosition.y)).x;
                    transform.position = newPosition;
                }
                else if (viewportPosition.x < 0 - _edgeOffsetX)
                {
                    newPosition.x = _camera.ViewportToWorldPoint(new Vector2(1 + _edgeRepositionOffsetX, viewportPosition.y)).x;
                    transform.position = newPosition;
                }
                else if (viewportPosition.y > 1 + _edgeOffsetY)
                {
                    newPosition.y = _camera.ViewportToWorldPoint(new Vector2(viewportPosition.x, -_edgeRepositionOffsetY)).y;
                    transform.position = newPosition;
                }
                else if (viewportPosition.y < 0 - _edgeOffsetY)
                {
                    newPosition.y = _camera.ViewportToWorldPoint(new Vector2(viewportPosition.x, 1 + _edgeRepositionOffsetY)).y;
                    transform.position = newPosition;
                }

                yield return new WaitForSeconds(_checkInterval);
            }
        }
        #endregion
    }
}