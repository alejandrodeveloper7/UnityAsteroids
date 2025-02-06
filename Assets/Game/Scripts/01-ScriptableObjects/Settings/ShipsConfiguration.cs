using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShipSettings", menuName = "ScriptableObjects/Settings/ShipSettings")]
public class ShipsConfiguration : ScriptableObject
{ 
    public List<SO_Ship> Ships;
}
