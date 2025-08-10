using UnityEngine;

[CreateAssetMenu(fileName = "InputSettings", menuName = "ScriptableObjects/Settings/InputSettings")]
public class InputSettings : ScriptableObject
{
    [Header("Controls")]
    public KeyCode MoveForwardKey;
    public KeyCode TurnLeftKey;
    public KeyCode TurnRightKey;
    [Space]
    public KeyCode ShootKey;
    [Space]
    public KeyCode PauseKey;
}
