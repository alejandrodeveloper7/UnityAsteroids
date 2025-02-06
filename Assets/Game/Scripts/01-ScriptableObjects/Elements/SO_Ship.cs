using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewShip", menuName = "ScriptableObjects/Elements/Ship")]
public class SO_Ship : SO_Selectable
{
    [Space]
    public string PoolName;

    [Header("Initialization")]
    public Vector3 InitialPostion;
    public Vector3 InitialRotation;

    [Header("Movement")]
    public float movementSpeed;
    public float rotationSpeed;

    [Header("Propulsion Fire")]
    public float PropulsionFireTransitionDuration;

    [Header("Configuration")]
    public Vector3 BulletsSpawnPointsLocalPosition;
    public Vector3 PropulsionFireLocalPosition;
    [Space]
    public List<ParticleSetup> DestuctionParticles;

    [Header("Edge Resposition")]
    public float EdgeOffsetY;
    public float EdgeRepositionOffsetY;
    [Space]
    public float EdgeOffsetX;
    public float EdgeRepositionOffsetX;

    [Header("Shield")]
    public float InvulnerabilityDuration;
    public Color ShieldColor;
    [Space]
    public float FadeInDuration;
    public float FadeOutDuration;
    [Space]
    public float BlickDuration;
    [Space]
    public float BlinkMinAlpha;
    public float BlinkMaxAlpha;

    [Header("Sound")]
    public SO_Sound[] SoundsOnShieldUp;
    public SO_Sound[] SoundsOnShieldDown;
    [Space]
    public SO_Sound[] SoundsOnDamage;
    public SO_Sound[] SoundsOnDestruction;
}
