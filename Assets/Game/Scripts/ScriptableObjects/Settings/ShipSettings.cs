using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShipSettings", menuName = "ScriptableObjects/Setting/ShipSettings", order = 3)]
public class ShipSettings : ScriptableObject
{ 
    public List<SO_Ship> Ships;
}
