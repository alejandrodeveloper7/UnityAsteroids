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
        _inputSettings = ResourcesManager.Instance.InputSettings;
    }

    private void OnEnable()
    {
        EventManager.GetGameplayBus().AddListener<StartMatch>(OnStartMatch);
    }

    private void OnDisable()
    {
        EventManager.GetGameplayBus().RemoveListener<StartMatch>(OnStartMatch);
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
            return;

        EventManager.GetGameplayBus().RaiseEvent(new RotationtKeyStateChange() { Value = 0 });
        EventManager.GetGameplayBus().RaiseEvent(new ShootKeyStateChange() { State = false });
        EventManager.GetGameplayBus().RaiseEvent(new MoveForwardKeyStateChange() { State = false });
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

    #endregion

    #region Input checks

    private void CheckRotationInput()
    {
        if (Input.GetKeyDown(_inputSettings.TurnLeftKey))
            EventManager.GetGameplayBus().RaiseEvent(new RotationtKeyStateChange() { Value = 1 });
       
        if (Input.GetKeyDown(_inputSettings.TurnRightKey))
            EventManager.GetGameplayBus().RaiseEvent(new RotationtKeyStateChange() { Value = -1 });

        if (Input.GetKeyUp(_inputSettings.TurnLeftKey))
            EventManager.GetGameplayBus().RaiseEvent(new RotationtKeyStateChange() { Value = 0 });

        if (Input.GetKeyUp(_inputSettings.TurnRightKey))
            EventManager.GetGameplayBus().RaiseEvent(new RotationtKeyStateChange() { Value = 0 });
    }

    private void CheckMovementInput()
    {
        if (Input.GetKeyDown(_inputSettings.MoveForwardKey))
            EventManager.GetGameplayBus().RaiseEvent(new MoveForwardKeyStateChange() { State = true });

        if (Input.GetKeyUp(_inputSettings.MoveForwardKey))
            EventManager.GetGameplayBus().RaiseEvent(new MoveForwardKeyStateChange() { State = false });
    }

    private void CheckShootInput()
    {
        if (Input.GetKeyDown(_inputSettings.ShootKey))
            EventManager.GetGameplayBus().RaiseEvent(new ShootKeyStateChange() { State = true });

        if (Input.GetKeyUp(_inputSettings.ShootKey))
            EventManager.GetGameplayBus().RaiseEvent(new ShootKeyStateChange() { State = false });
    }

    private void CheckPauseInput()
    {
        if (Input.GetKeyDown(_inputSettings.PauseKey))
        {
            _inPause = !_inPause;
            EventManager.GetGameplayBus().RaiseEvent(new PauseKeyClicked() { InPause = _inPause });
        }
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
    public bool InPause { get; set; }
}

#endregion
