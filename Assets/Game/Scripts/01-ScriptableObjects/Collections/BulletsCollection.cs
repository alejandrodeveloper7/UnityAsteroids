using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletsCollection", menuName = "ScriptableObjects/Coollections/BulletsCollection")]
public class BulletsCollection : ScriptableObject
{
    public List<SO_Bullet> Bullets;  
}
