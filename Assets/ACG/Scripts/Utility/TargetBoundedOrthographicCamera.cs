using System.Collections.Generic;
using UnityEngine;

namespace ACG.Scripts.Utilitys
{
    public class TargetBoundedOrthographicCamera : MonoBehaviour
    {
        #region Fields

        [Header("Configuration")]
        [SerializeField] private bool _dynamicTargets = false;
        [SerializeField] private bool _drawGizmos = false;

        [Header("References")]
        [SerializeField] private Camera _orthographicCamera;
        [SerializeField] private List<Transform> _targets;
        public Vector3 CalculatedPosition { get; private set; }

        [Header("Settings")]
        [SerializeField] private float _topMargin = 2f;
        [SerializeField] private float _bottomMargin = 2f;
        [SerializeField] private float _leftMargin = 2f;
        [SerializeField] private float _rightMargin = 2f;

        #endregion

        #region Monobehaviour

        private void Start()
        {
            if (_orthographicCamera == null)
                _orthographicCamera = Camera.main;

            if (_orthographicCamera.orthographic == false)
                Debug.LogError("The camera is not orthographic");
        }

        private void Update()
        {
            if (_dynamicTargets)
                UpdateCameraSize();
        }

        private void OnDrawGizmos()
        {
            if (_drawGizmos is false)
                return;

            if (_targets == null || _targets.Count == 0)
                return;

            Gizmos.color = Color.red;

            foreach (Transform target in _targets)
                Gizmos.DrawSphere(target.position, 0.1f);

            Gizmos.color = Color.green;

            Vector3 bottomLeft =
                    new(
                        _orthographicCamera.transform.position.x -
                        _orthographicCamera.aspect * _orthographicCamera.orthographicSize,
                        _orthographicCamera.transform.position.y - _orthographicCamera.orthographicSize, 0);

            Vector3 topRight =
                    new(
                        _orthographicCamera.transform.position.x +
                        _orthographicCamera.aspect * _orthographicCamera.orthographicSize,
                        _orthographicCamera.transform.position.y + _orthographicCamera.orthographicSize, 0);

            Gizmos.DrawLine(bottomLeft, new(bottomLeft.x, topRight.y, 0));
            Gizmos.DrawLine(bottomLeft, new(topRight.x, bottomLeft.y, 0));
            Gizmos.DrawLine(topRight, new(bottomLeft.x, topRight.y, 0));
            Gizmos.DrawLine(topRight, new(topRight.x, bottomLeft.y, 0));
        }

        #endregion

        #region Functionality

        private void UpdateCameraSize()
        {
            if (_targets == null || _targets.Count == 0)
                return;

            float minX = float.MaxValue;
            float minY = float.MaxValue;
            float maxX = float.MinValue;
            float maxY = float.MinValue;

            foreach (var target in _targets)
            {
                if (target.position.x < minX)
                    minX = target.position.x;

                if (target.position.y < minY)
                    minY = target.position.y;

                if (target.position.x > maxX)
                    maxX = target.position.x;

                if (target.position.y > maxY)
                    maxY = target.position.y;
            }

            float width = maxX - minX + _leftMargin + _rightMargin;
            float height = maxY - minY + _topMargin + _bottomMargin;

            float aspectRatio = _orthographicCamera.aspect;
            float cameraSize = Mathf.Max(width / (2 * aspectRatio), height / 2);

            _orthographicCamera.orthographicSize = cameraSize;

            Vector3 cameraCenter = new((minX + maxX) / 2, (minY + maxY) / 2, _orthographicCamera.transform.position.z);
            _orthographicCamera.transform.position = cameraCenter;
            CalculatedPosition = cameraCenter;
        }

        #endregion

        #region Target Management

        public void SetTargets(List<Transform> newTargets)
        {
            _targets = newTargets;
            UpdateCameraSize();
        }

        public void AddTarget(Transform target)
        {
            _targets ??= new List<Transform>();

            _targets.Add(target);
            UpdateCameraSize();
        }

        public void RemoveTarget(Transform target)
        {
            if (_targets == null)
                return;

            _targets.Remove(target);
            UpdateCameraSize();
        }

        public void AddTargets(List<Transform> targets)
        {
            _targets ??= new List<Transform>();

            _targets.AddRange(targets);
            UpdateCameraSize();
        }

        public void RemoveTargets(List<Transform> targets)
        {
            if (_targets == null)
                return;

            foreach (Transform target in targets)
                _targets.Remove(target);

            UpdateCameraSize();
        }

        public void ClearTargets()
        {
            if (_targets == null)
                return;

            _targets.Clear();
            UpdateCameraSize();
        }

        #endregion
    }
}