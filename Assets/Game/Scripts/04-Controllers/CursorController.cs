using System.Threading.Tasks;
using ToolsACG.Utils.Events;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    #region Fields

    private CursorConfiguration _cursorConfiguration;
    [Space]
    private bool _isClicked = false;
    private bool _isLocked = false;
    private bool _isVisible = true;

    #endregion

    #region Monobehaviour

    private void Awake()
    {
        _cursorConfiguration = ResourcesManager.Instance.GetScriptableObject<CursorConfiguration>(ScriptableObjectKeys.CURSOR_CONFIGURATION_KEY);
    }

    private void Start()
    {
        SetCursorTexture(_cursorConfiguration.DefaultCursor);
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
            SetCursorTexture(_cursorConfiguration.ClickedCursor);
        }
        else if (Input.GetMouseButtonUp(0) && _isClicked)
        {
            _isClicked = false;
            SetCursorTexture(_cursorConfiguration.DefaultCursor);
        }
    }

    private void OnEnable()
    {
        EventManager.GameplayBus.AddListener<StartMatch>(OnStartMatch);
        EventManager.GameplayBus.AddListener<PauseStateChanged>(OnPauseStateChanged);
        EventManager.GameplayBus.AddListener<PlayerDead>(OnPlayerDead);
        EventManager.UIBus.AddListener<GameLeaved>(OnGameLeaved);
    }

    private void OnDisable()
    {
        EventManager.GameplayBus.RemoveListener<StartMatch>(OnStartMatch);
        EventManager.GameplayBus.RemoveListener<PauseStateChanged>(OnPauseStateChanged);
        EventManager.GameplayBus.RemoveListener<PlayerDead>(OnPlayerDead);
        EventManager.UIBus.RemoveListener<GameLeaved>(OnGameLeaved);
    }

    #endregion

    #region Bus Callbacks

    private void OnPlayerDead(PlayerDead pPlayerDead)
    {
        TurnCursor(true);
    }
    private void OnStartMatch(StartMatch pStartMatch)
    {
        TurnCursor(false);
    }
    private void OnPauseStateChanged(PauseStateChanged pPauseStateChanged)
    {
        if (pPauseStateChanged.InPause)
            TurnCursor(true);
        else
            TurnCursor(false);
    }
    private async void OnGameLeaved(GameLeaved pGameLeaved) 
    {
        await Task.Yield();
        TurnCursor(true);
    }

    #endregion

    #region Funcionality

    private void SetCursorTexture(Texture2D pImage)
    {
        Cursor.SetCursor(pImage, _cursorConfiguration.Hotspot, CursorMode.Auto);
    }

    private void TurnCursor(bool pState)
    {
        SetCursorVisibility(pState);
        LockCursor(!pState);
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
