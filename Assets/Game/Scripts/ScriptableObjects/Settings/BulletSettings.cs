using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletSettings", menuName = "ScriptableObjects/Setting/BulletSettings", order = 4)]
public class BulletSettings : ScriptableObject
{
    public List<SO_Bullet> Bullets;  
}
