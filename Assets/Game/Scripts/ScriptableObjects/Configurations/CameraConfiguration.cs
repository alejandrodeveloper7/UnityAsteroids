using UnityEngine;

[CreateAssetMenu(fileName = "CameraConfiguration", menuName = "ScriptableObjects/Configurations/CameraConfiguration", order = 3)]
public class CameraConfiguration : ScriptableObject
{
    public float ShieldShakeMagnitude;
    public float ShieldShakeDuration;
    [Space]
    public float DamageShakeMagnitude;
    public float DamageShakeDuration;
    [Space]
    public float DeadShakeMagnitude;
    public float DeadShakeDuration;
    [Space(20)]
    public float MinDistance;
    public float MaxDistance;

}
