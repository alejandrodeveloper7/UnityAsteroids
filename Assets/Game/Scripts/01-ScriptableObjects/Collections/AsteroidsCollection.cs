using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AsteroidsCollection", menuName = "ScriptableObjects/Collections/AsteroidsCollection")]
public class AsteroidsCollection : ScriptableObject
{ 
    public List<SO_Asteroid> Asteroids;
}
