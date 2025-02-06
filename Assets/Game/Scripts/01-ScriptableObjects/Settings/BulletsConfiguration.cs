using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletsConfiguration", menuName = "ScriptableObjects/Configurations/BulletsConfiguration")]
public class BulletsConfiguration : ScriptableObject
{
    public List<SO_Bullet> Bullets;  
}
