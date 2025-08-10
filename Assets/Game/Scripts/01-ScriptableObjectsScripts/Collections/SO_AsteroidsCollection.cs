using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AsteroidsCollection", menuName = "ScriptableObjects/Collections/AsteroidsCollection")]
public class SO_AsteroidsCollection : ScriptableObject
{ 
    public List<SO_Asteroid> Asteroids;
}
