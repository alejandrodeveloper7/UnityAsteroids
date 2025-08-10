using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShipCollection", menuName = "ScriptableObjects/Collections/ShipsCollection")]
public class ShipsCollection : ScriptableObject
{ 
    public List<SO_Ship> Ships;
}
