using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "ScriptableObjects/Settings/PlayerSettings")]
public class PlayerSettings : ScriptableObject
{
    [Header("Health")]
    [Range(1,5)]public int HealthPoints;
    [Space]
    public float ShieldRecoveryTime;
}
