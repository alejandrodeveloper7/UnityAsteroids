using System.Collections;
using UnityEngine;

public class ScreenEdgeTeleport : MonoBehaviour
{
    #region Fields

    [Header("Settings")]
    private Camera _camera;

    [SerializeField] private float _edgeOffset = 0.03f;
    [SerializeField] private float _edgeRepositionOffset = 0.02f;

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
            Vector3 newPosition = transform.position;

            if (viewportPosition.x > 1+_edgeOffset)
            {
                newPosition.x = _camera.ViewportToWorldPoint(new Vector2(-_edgeRepositionOffset, viewportPosition.y)).x;
                transform.position = newPosition;
            }
            else if (viewportPosition.x < 0 - _edgeOffset)
            {
                newPosition.x = _camera.ViewportToWorldPoint(new Vector2(1+_edgeRepositionOffset, viewportPosition.y)).x;
                transform.position = newPosition;
            }
            else if (viewportPosition.y > 1 + _edgeOffset)
            {
                newPosition.y = _camera.ViewportToWorldPoint(new Vector2(viewportPosition.x ,- _edgeRepositionOffset)).y;
                transform.position = newPosition;
            }
            else if (viewportPosition.y < 0- _edgeOffset)
            {
                newPosition.y = _camera.ViewportToWorldPoint(new Vector2(viewportPosition.x, 1+_edgeRepositionOffset)).y;
                transform.position = newPosition;
            }
            yield return new WaitForSeconds(0.05f);
        }
    }

    #endregion
}
