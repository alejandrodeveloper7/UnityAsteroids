using System.Collections;
using ToolsACG.Utils.Events;
using UnityEngine;

public class CameraController : MonoBehaviour
{
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
        StartCoroutine(ShakeCoroutine(0.4f, 0.3f));
    }
    private void OnPlayerDamaged(PlayerDamaged pPlayerDamaged)
    {
        StartCoroutine(ShakeCoroutine(0.3f, 0.2f));
    }
    private void OnShieldStateChanged(ShieldStateChanged pShieldStateChanged)
    {
        if (pShieldStateChanged.Active is false)
            StartCoroutine(ShakeCoroutine(0.2f, 0.1f));
    }

    private IEnumerator ShakeCoroutine(float pDuration, float pMagnitude)
    {
        float elapsed = 0f;

        while (elapsed < pDuration)
        {
            float offsetX = Random.Range(-1f, 1f) * pMagnitude;
            float offsetY = Random.Range(-1f, 1f) * pMagnitude;

            transform.localPosition = _originalPosition + new Vector3(offsetX, offsetY, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = _originalPosition;
    }
}
