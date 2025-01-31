using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AsteroidsSettings", menuName = "ScriptableObjects/Setting/AsteroidSettings", order = 2)]
public class AsteroidSettings : ScriptableObject
{
    public GameObject Prefab;
    public float BrokenAsteroidSpawnAngle;
    [Space]
    public List<SO_Asteroid> Asteroids;
}
