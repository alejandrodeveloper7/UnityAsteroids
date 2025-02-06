using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AsteroidsConfiguration", menuName = "ScriptableObjects/Configurations/AsteroidsConfiguration")]
public class AsteroidsConfiguration : ScriptableObject
{ 
    public List<SO_Asteroid> Asteroids;
}
