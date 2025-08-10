using UnityEngine;

[CreateAssetMenu(fileName = "CameraConfiguration", menuName = "ScriptableObjects/Configurations/CameraConfiguration")]
public class SO_CameraConfiguration : ScriptableObject
{
    [Header("General shake Configuration")]
    public float MinDistance;
    public float MaxDistance;

    [Header("Shield shake")]
    public float ShieldShakeMagnitude;
    public float ShieldShakeDuration;

    [Header("Damage shake")]
    public float DamageShakeMagnitude;
    public float DamageShakeDuration;

    [Header("Dead shake")]
    public float DeadShakeMagnitude;
    public float DeadShakeDuration;

}
