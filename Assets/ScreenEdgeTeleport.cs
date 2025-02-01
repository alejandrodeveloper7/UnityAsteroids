using System.Collections;
using UnityEngine;

public class ScreenEdgeTeleport : MonoBehaviour
{
    #region Fields

    [Header("Settings")]
    private Camera _camera;

    private Coroutine _currentRelocationCoroutine;

    #endregion

    #region Monobehaviour

    private void Awake()
    {
        _camera = Camera.main;
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
            Vector3 newPosition;

            if (viewportPosition.x > 1.025f || viewportPosition.x < -0.025f)
            {
                newPosition = transform.position;
                newPosition.x *= -0.98f;
                transform.position = newPosition;
            }
            else if (viewportPosition.y > 1.03f || viewportPosition.y < -0.03f)
            {
                newPosition = transform.position;
                newPosition.y *= -0.98f;
                transform.position = newPosition;
            }

            yield return new WaitForSeconds(0.05f);
        }
    }

    #endregion
}
