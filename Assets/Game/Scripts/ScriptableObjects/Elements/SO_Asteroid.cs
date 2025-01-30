using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAsteroid", menuName = "ScriptableObjects/Elements/Asteroid", order = 0)]
public class SO_Asteroid : ScriptableObject
{
    public int Id;
    [Space]
    public GameObject Prefab;
    [Space]
    public string colorType; //Use a string instead a enum allows scale without recompile
    public Sprite[] possibleSprites;
    [Space]
    public int Speed;
    public int PointsValue;
    [Space]
    public List<ParticleSetup> DestuctionParticles;
    public SO_Asteroid[] fragmentTypes;
}
