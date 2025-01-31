using UnityEngine;

public class InputManager : MonoBehaviour
{
    private InputSettings _inputSettings;

    private bool _playing;
    private bool _gamePaused;

    #region Monobehaviour

    private void Awake()
    {
        _inputSettings = ResourcesManager.Instance.InputSettings;
    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }


    private void Update()
    {
        if (_playing is false)
            return;

        if (_gamePaused)
            return;

        CheckInputs();
    }

    #endregion

    #region Bus Callbacks
    
    
    
    #endregion

    private void CheckInputs()
    {

    }
}
