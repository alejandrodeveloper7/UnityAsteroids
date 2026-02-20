using ACG.Tools.Runtime.ManagersCreator.Bases;
using System.Collections;
using UnityEngine;
using ACG.Scripts.Models;

namespace ACG.Scripts.Managers
{
    public class CameraFXManager : MonobehaviourInstancesManagerBase<CameraFXManager>, ICameraFXManager
    {
        #region Fields

        [Header("References")]
        private Camera _camera;
        private Vector3 _originalPosition;

        #endregion

        #region Initialization

        protected override void GetReferences()
        {
            base.GetReferences();

            _camera = Camera.main;

            if (_camera != null)
                _originalPosition = _camera.transform.position;
        }

        public void Setup()
        {
            // TODO: Manual method to set parameters
        }

        public override void Initialize()
        {
            base.Initialize();
            // TODO: Method for initial logic and event subscriptions (called by Zenject)
        }

        public override void Dispose()
        {
            base.Dispose();
            // TODO: Clean here all the listeners or elements that need be clean when the script is destroyed (called by Zenject)
        }

        #endregion

        #region Monobehaviour

        protected override void Awake()
        {
            base.Awake();

            GetReferences();
        }

        protected override void Start()
        {
            base.Start();
            // TODO: Add start logic here
        }

        #endregion

        #region Functionality

        public void PlayCameraShake(CameraShakeData shakeData)
        {
            StartCoroutine(CameraShakeCoroutine(shakeData));
        }

        private IEnumerator CameraShakeCoroutine(CameraShakeData shakeData)
        {
            _camera.transform.localPosition = _originalPosition;

            float elapsed = 0f;

            while (elapsed < shakeData._shieldShakeDuration)
            {
                float offsetX = Random.Range(shakeData._shieldMinDistance, shakeData._shieldMaxDistance) * shakeData._shieldShakeMagnitude;
                float offsetY = Random.Range(shakeData._shieldMinDistance, shakeData._shieldMaxDistance) * shakeData._shieldShakeMagnitude;
                _camera.transform.localPosition = _originalPosition + new Vector3(offsetX, offsetY, 0);

                elapsed += Time.deltaTime;
                yield return null;
            }
            _camera.transform.localPosition = _originalPosition;
        }

        #endregion
    }
}
