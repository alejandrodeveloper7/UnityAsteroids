using UnityEngine;

[CreateAssetMenu(fileName = "InputSettings", menuName = "ScriptableObjects/Setting/InputSettings",order = 0)]
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
