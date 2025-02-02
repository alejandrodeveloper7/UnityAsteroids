using UnityEngine;

public class CursorController : MonoBehaviour
{
    #region Fields

    [SerializeField]private CursorConfiguration _configuration;
    [Space]
    private bool _isClicked = false;
    private bool _isLocked = false;
    private bool _isVisible = true;

    #endregion

    #region Monobehaviour

    private void Start()
    {
        SetCursorTexture(_configuration.DefaultCursor);
    }

    private void Update()
    {
        if (_isVisible is false)
            return;
        if (_isLocked)
            return;

        if (Input.GetMouseButtonDown(0) && _isClicked is false)
        {
            _isClicked = true;
            SetCursorTexture(_configuration.ClickedCursor);
        }
        else if (Input.GetMouseButtonUp(0) && _isClicked)
        {
            _isClicked = false;
            SetCursorTexture(_configuration.DefaultCursor);
        }
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    #endregion

    #region Funcionality

    private void SetCursorTexture(Texture2D pImage)
    {
        Cursor.SetCursor(pImage, _configuration.Hotspot, CursorMode.Auto);
    }

    private void SetCursorVisibility(bool pState)
    {
        _isVisible = pState;
        Cursor.visible = pState;
    }

    private void LockCursor(bool pLocked)
    {
        _isLocked = pLocked;

        if (_isLocked)
            Cursor.lockState = CursorLockMode.Locked;
        else
            Cursor.lockState = CursorLockMode.None;
    }

    #endregion
}
