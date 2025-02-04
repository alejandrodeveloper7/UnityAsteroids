using System.Collections;
using ToolsACG.Utils.Events;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]private CameraConfiguration _cameraConfiguration;
    private Vector3 _originalPosition;

    private void Awake()
    {
        _originalPosition = transform.position;
    }

    private void OnEnable()
    {
        EventManager.GetGameplayBus().AddListener<PlayerDead>(OnPlayerDead);
        EventManager.GetGameplayBus().AddListener<PlayerDamaged>(OnPlayerDamaged);
        EventManager.GetGameplayBus().AddListener<ShieldStateChanged>(OnShieldStateChanged);
    }

    private void OnDisable()
    {
        EventManager.GetGameplayBus().RemoveListener<PlayerDead>(OnPlayerDead);
        EventManager.GetGameplayBus().RemoveListener<PlayerDamaged>(OnPlayerDamaged);
        EventManager.GetGameplayBus().RemoveListener<ShieldStateChanged>(OnShieldStateChanged);
    }

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

    private IEnumerator ShakeCoroutine(float pDuration, float pMagnitude)
    {
        float elapsed = 0f;

        while (elapsed < pDuration)
        {
            float offsetX = Random.Range(_cameraConfiguration.MinDistance, _cameraConfiguration.MaxDistance) * pMagnitude;
            float offsetY = Random.Range(_cameraConfiguration.MinDistance, _cameraConfiguration.MaxDistance) * pMagnitude;
            transform.localPosition = _originalPosition + new Vector3(offsetX, offsetY, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = _originalPosition;
    }
}
