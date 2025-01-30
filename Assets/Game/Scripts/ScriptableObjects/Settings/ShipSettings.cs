using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShipSettings", menuName = "ScriptableObjects/Setting/ShipSettings", order = 3)]
public class ShipSettings : ScriptableObject
{
    //Common setting for all ships there

    public List<SO_Ship> Ships;
}
