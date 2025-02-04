using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewShip", menuName = "ScriptableObjects/Elements/Ship", order = 1)]
public class SO_Ship : SO_Selectable
{
    [Space]
    public string PoolName;
    [Space]
    public Vector3 BulletsSpawnPointsLocalPosition;
    public Vector3 PropulsionFireLocalPosition;
    [Space]
    public SO_Sound[] SoundsOnShieldUp;
    public SO_Sound[] SoundsOnShieldDown;
    public SO_Sound[] SoundsOnDamage;
    public SO_Sound[] SoundsOnDestruction;
    public float movementSpeed;
    public float rotationSpeed;
    [Space]
    public List<ParticleSetup> DestuctionParticles;
    [Space]
    public Color ShieldColor;
    [Space]
    public float FadeInDuration;
    public float FadeOutDuration;
    [Space]
    public float BlickDuration;
    [Space]
    public float BlinkMinAlpha;
    public float BlinkMaxAlpha;
}
