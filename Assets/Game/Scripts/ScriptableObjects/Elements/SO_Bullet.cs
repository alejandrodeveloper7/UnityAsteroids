using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBullet", menuName = "ScriptableObjects/Elements/Bullet", order = 2)]
public class SO_Bullet : SO_Selectable
{ 
    [Space]
    public string PoolName;
    [Space]
    public SO_Sound[] SoundsOnShoot;
    public float BetweenBulletsTime;
    public float LifeDuration;
    public float Speed;
    [Space]
    public List<ParticleSetup> DestuctionParticles;
}
