using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAsteroid", menuName = "ScriptableObjects/Elements/Asteroid", order = 0)]
public class SO_Asteroid : SO_BaseElement
{
    public bool IsInitialAsteroid;
    [Space]
    public string PoolName;
    public float BrokenAsteroidSpawnAngle;
    [Space]
    public Sprite[] possibleSprites;
    [Space]
    public int Speed;
    public int PointsValue;
    [Space]
    public List<ParticleSetup> DestuctionParticles;
    [Space]
    public int FragmentsAmountGeneratedOnDestruction;
    public List<SO_Asteroid> fragmentTypes;
}
