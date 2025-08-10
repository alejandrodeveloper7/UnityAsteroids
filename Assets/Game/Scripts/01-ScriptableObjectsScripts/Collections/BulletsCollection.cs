using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletsCollection", menuName = "ScriptableObjects/Collections/BulletsCollection")]
public class BulletsCollection : ScriptableObject
{
    public List<SO_Bullet> Bullets;  
}
