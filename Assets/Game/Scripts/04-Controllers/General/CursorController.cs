using UnityEngine;

public class CursorController : MonoBehaviour
{
    #region Fields

    [SerializeField] private SO_CursorConfiguration _cursorConfiguration;
    [Space]
    private bool _isClicked = false;

    #endregion

    #region

    private void Initialize()
    {
        if (_cursorConfiguration == null)
        {
            Debug.LogWarning("Cursor configuration not set on CursorController");
            enabled = false;
            return;
        }

        CursorManager.SetCursorTexture(_cursorConfiguration.DefaultCursor, _cursorConfiguration.Hotspot);
        CursorManager.OnCursorBecomeVisible += SetCursorToDefault;
    }

    private void OnDestroy()
    {
        CursorManager.OnCursorBecomeVisible -= SetCursorToDefault;
    }

    #endregion

    #region Monobehaviour

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        if (CursorManager.IsVisible is false)
            return;

        if (Input.GetMouseButtonDown(0) && _isClicked is false)
        {
            _isClicked = true;
            CursorManager.SetCursorTexture(_cursorConfiguration.ClickedCursor, _cursorConfiguration.Hotspot);
        }
        else if (Input.GetMouseButtonUp(0) && _isClicked)
        {
            _isClicked = false;
            CursorManager.SetCursorTexture(_cursorConfiguration.DefaultCursor, _cursorConfiguration.Hotspot);
        }
    }

    #endregion

    #region Functionality

    public void SetCursorToDefault()
    {
        CursorManager.SetCursorTexture(_cursorConfiguration.DefaultCursor, _cursorConfiguration.Hotspot);
    }

    public void SetCursorToClicked()
    {
        CursorManager.SetCursorTexture(_cursorConfiguration.ClickedCursor, _cursorConfiguration.Hotspot);
    }

    #endregion
}
