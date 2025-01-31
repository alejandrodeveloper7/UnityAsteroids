using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAsteroid", menuName = "ScriptableObjects/Elements/Asteroid", order = 0)]
public class SO_Asteroid : SO_BaseElement
{  
    [Space]
    public Sprite[] possibleSprites;
    [Space]
    public int Speed;
    public int PointsValue;
    [Space]
    public List<ParticleSetup> DestuctionParticles;
    public SO_Asteroid[] fragmentTypes;
}
