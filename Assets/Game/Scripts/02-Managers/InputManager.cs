using ToolsACG.Utils.Events;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private InputSettings _inputSettings;

    private bool _playing;
    private bool _inPause;

    #region Monobehaviour

    private void Awake()
    {
        _inputSettings = ResourcesManager.Instance.GetScriptableObject<InputSettings>(ScriptableObjectKeys.INPUT_SETTINGS_KEY);
    }

    private void OnEnable()
    {
        EventManager.GameplayBus.AddListener<StartMatch>(OnStartMatch);
        EventManager.GameplayBus.AddListener<PlayerDead>(OnPlayerDead);
        EventManager.GameplayBus.AddListener<PauseStateChanged>(OnPauseStateChanged);
        EventManager.UIBus.AddListener<GameLeaved>(OnGameLeaved);
    }

    private void OnDisable()
    {
        EventManager.GameplayBus.RemoveListener<StartMatch>(OnStartMatch);
        EventManager.GameplayBus.RemoveListener<PlayerDead>(OnPlayerDead);
        EventManager.GameplayBus.RemoveListener<PauseStateChanged>(OnPauseStateChanged);
        EventManager.UIBus.RemoveListener<GameLeaved>(OnGameLeaved);
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
            return;

        EventManager.InputBus.RaiseEvent(new RotationtKeyStateChange() { Value = 0 });
        EventManager.InputBus.RaiseEvent(new ShootKeyStateChange() { State = false });
        EventManager.InputBus.RaiseEvent(new MoveForwardKeyStateChange() { State = false });
    }


    private void Update()
    {
        if (_playing is false)
            return;

        CheckPauseInput();

        if (_inPause)
            return;

        CheckRotationInput();
        CheckMovementInput();
        CheckShootInput();
    }

    #endregion

    #region Bus Callbacks

    private void OnStartMatch(StartMatch pStartMatch)
    {
        _playing = true;
    }

    private void OnPlayerDead(PlayerDead pPlayerDead)
    {
        _playing = false;
    }

    private void OnPauseStateChanged(PauseStateChanged pPauseStateChanged)
    {
        _inPause = pPauseStateChanged.InPause;

        if (_inPause)
            return;
        if (_playing is false)
            return;

        if (Input.GetKey(_inputSettings.TurnLeftKey))
            EventManager.InputBus.RaiseEvent(new RotationtKeyStateChange() { Value = 1 });

        if (Input.GetKey(_inputSettings.TurnRightKey))
            EventManager.InputBus.RaiseEvent(new RotationtKeyStateChange() { Value = -1 });

        if (Input.GetKey(_inputSettings.MoveForwardKey))
            EventManager.InputBus.RaiseEvent(new MoveForwardKeyStateChange() { State = true });

        if (Input.GetKey(_inputSettings.ShootKey))
            EventManager.InputBus.RaiseEvent(new ShootKeyStateChange() { State = true });
    }

    private void OnGameLeaved(GameLeaved pGameLeaved) 
    {
        _playing = false;
    }

    #endregion

    #region Input checks

    private void CheckRotationInput()
    {
        if (Input.GetKeyDown(_inputSettings.TurnLeftKey))
            EventManager.InputBus.RaiseEvent(new RotationtKeyStateChange() { Value = 1 });

        if (Input.GetKeyDown(_inputSettings.TurnRightKey))
            EventManager.InputBus.RaiseEvent(new RotationtKeyStateChange() { Value = -1 });

        if (Input.GetKeyUp(_inputSettings.TurnLeftKey))
        {
            if (Input.GetKey(_inputSettings.TurnRightKey))
                EventManager.InputBus.RaiseEvent(new RotationtKeyStateChange() { Value = -1 });
            else
                EventManager.InputBus.RaiseEvent(new RotationtKeyStateChange() { Value = 0 });
        }

        if (Input.GetKeyUp(_inputSettings.TurnRightKey))
        {
            if (Input.GetKey(_inputSettings.TurnLeftKey))
                EventManager.InputBus.RaiseEvent(new RotationtKeyStateChange() { Value = 1 });
            else
                EventManager.InputBus.RaiseEvent(new RotationtKeyStateChange() { Value = 0 });
        }
    }

    private void CheckMovementInput()
    {
        if (Input.GetKeyDown(_inputSettings.MoveForwardKey))
            EventManager.InputBus.RaiseEvent(new MoveForwardKeyStateChange() { State = true });

        if (Input.GetKeyUp(_inputSettings.MoveForwardKey))
            EventManager.InputBus.RaiseEvent(new MoveForwardKeyStateChange() { State = false });
    }

    private void CheckShootInput()
    {
        if (Input.GetKeyDown(_inputSettings.ShootKey))
            EventManager.InputBus.RaiseEvent(new ShootKeyStateChange() { State = true });

        if (Input.GetKeyUp(_inputSettings.ShootKey))
            EventManager.InputBus.RaiseEvent(new ShootKeyStateChange() { State = false });
    }

    private void CheckPauseInput()
    {
        if (Input.GetKeyDown(_inputSettings.PauseKey))
            EventManager.InputBus.RaiseEvent(new PauseKeyClicked());
    }

    #endregion
}

#region IEvents

public class RotationtKeyStateChange : IEvent
{
    public int Value { get; set; }
}

public class MoveForwardKeyStateChange : IEvent
{
    public bool State { get; set; }
}

public class ShootKeyStateChange : IEvent
{
    public bool State { get; set; }
}

public class PauseKeyClicked : IEvent
{
}

#endregion
