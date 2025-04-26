using System.Collections;
using UnityEngine;

public class ScreenEdgeTeleport : MonoBehaviour
{
    #region Fields

    [SerializeField] private float _EdgeOffsetY; public float EdgeOffsetY {  get { return _EdgeOffsetY; } set { _EdgeOffsetY = value; } }
    [SerializeField] private float _EdgeRepositionOffsetY; public float EdgeRepositionOffsetY { get { return _EdgeRepositionOffsetY; } set { _EdgeRepositionOffsetY = value; } }

    [SerializeField] private float _EdgeOffsetX; public float EdgeOffsetX { get { return _EdgeOffsetX; } set { _EdgeOffsetX = value; } }
    [SerializeField] private float _EdgeRepositionOffsetX; public float EdgeRepositionOffsetX { get { return _EdgeRepositionOffsetX; } set { _EdgeRepositionOffsetX = value; } }

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
            Vector3 newPosition = transform.position;

            if (viewportPosition.x > 1+_EdgeOffsetX)
            {
                newPosition.x = _camera.ViewportToWorldPoint(new Vector2(-_EdgeRepositionOffsetX, viewportPosition.y)).x;
                transform.position = newPosition;
            }
            else if (viewportPosition.x < 0 - _EdgeOffsetX)
            {
                newPosition.x = _camera.ViewportToWorldPoint(new Vector2(1+_EdgeRepositionOffsetX, viewportPosition.y)).x;
                transform.position = newPosition;
            }
            else if (viewportPosition.y > 1 + _EdgeOffsetY)
            {
                newPosition.y = _camera.ViewportToWorldPoint(new Vector2(viewportPosition.x ,- _EdgeRepositionOffsetY)).y;
                transform.position = newPosition;
            }
            else if (viewportPosition.y < 0- _EdgeOffsetY)
            {
                newPosition.y = _camera.ViewportToWorldPoint(new Vector2(viewportPosition.x, 1+_EdgeRepositionOffsetY)).y;
                transform.position = newPosition;
            }
            yield return new WaitForSeconds(0.05f);
        }
    }

    #endregion
}
