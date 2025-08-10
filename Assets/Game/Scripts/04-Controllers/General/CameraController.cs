using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]private SO_CameraConfiguration _cameraConfiguration;
    [SerializeField] private Camera _camera;
    private Vector3 _originalPosition;

    #region Monobehaviour

    private void Awake()
    {
        GetReferences();
    }

    private void OnEnable()
    {
        EventManager.GameplayBus.AddListener<PlayerDead>(OnPlayerDead);
        EventManager.GameplayBus.AddListener<PlayerDamaged>(OnPlayerDamaged);
        EventManager.GameplayBus.AddListener<ShieldStateChanged>(OnShieldStateChanged);
    }

    private void OnDisable()
    {
        EventManager.GameplayBus.RemoveListener<PlayerDead>(OnPlayerDead);
        EventManager.GameplayBus.RemoveListener<PlayerDamaged>(OnPlayerDamaged);
        EventManager.GameplayBus.RemoveListener<ShieldStateChanged>(OnShieldStateChanged);
    }

    #endregion

    #region Bus callbacks

    private void OnPlayerDead(PlayerDead pPlayerDead)
    {
        StartCoroutine(ShakeCoroutine(_cameraConfiguration.DeadShakeDuration, _cameraConfiguration.DeadShakeMagnitude));
    }
    private void OnPlayerDamaged(PlayerDamaged pPlayerDamaged)
    {
        StartCoroutine(ShakeCoroutine(_cameraConfiguration.DamageShakeDuration, _cameraConfiguration.DamageShakeMagnitude));
    }
    private void OnShieldStateChanged(ShieldStateChanged pShieldStateChanged)
    {
        if (pShieldStateChanged.Active is false)
            StartCoroutine(ShakeCoroutine(_cameraConfiguration.ShieldShakeDuration, _cameraConfiguration.ShieldShakeMagnitude));
    }

    #endregion

    #region Initialization

    private void GetReferences() 
    {
        _originalPosition = _camera.transform.position;
    }

    #endregion

    #region Functionality

    private IEnumerator ShakeCoroutine(float pDuration, float pMagnitude)
    {
        float elapsed = 0f;

        while (elapsed < pDuration)
        {
            float offsetX = Random.Range(_cameraConfiguration.MinDistance, _cameraConfiguration.MaxDistance) * pMagnitude;
            float offsetY = Random.Range(_cameraConfiguration.MinDistance, _cameraConfiguration.MaxDistance) * pMagnitude;
            _camera.transform.localPosition = _originalPosition + new Vector3(offsetX, offsetY, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }
        _camera.transform.localPosition = _originalPosition;
    }

    #endregion
}
