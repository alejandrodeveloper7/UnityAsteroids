using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAsteroid", menuName = "ScriptableObjects/Elements/Asteroid")]
public class SO_Asteroid : SO_Base
{
    public string PoolName;
    [Space]
    public bool IsInitialAsteroid;
    public Sprite[] possibleSprites;

    [Header("Configuration")]
    public int MaxHP;
    public float AsteroidSpawnAngle;
    public float PosibleTorque;
    public int Speed;
    [Space]
    public int PointsValue;

    [Header("Edge Resposition")]
    public float EdgeOffsetY;
    public float EdgeRepositionOffsetY;
    [Space]
    public float EdgeOffsetX;
    public float EdgeRepositionOffsetX;
    
    [Header("Destruction")]
    public List<ParticleSetup> DestuctionParticles;
    public int FragmentsAmountGeneratedOnDestruction;
    public List<SO_Asteroid> fragmentTypes;
    public SO_Sound[] SoundsOnDestruction;
}
