using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletSettings", menuName = "ScriptableObjects/Setting/BulletSettings", order = 4)]
public class BulletSettings : ScriptableObject
{
    //Common setting for all Bullets there

    public List<SO_Bullet> Bullets;  
}
