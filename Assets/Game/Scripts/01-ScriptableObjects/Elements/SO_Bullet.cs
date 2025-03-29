using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBullet", menuName = "ScriptableObjects/Elements/Bullet")]
public class SO_Bullet : SO_Selectable
{ 
    [Space]
    public string PoolName;

    [Header("Configuration")]
    public Color Color;
    public float Speed;
    public float LifeDuration;
    public float BetweenBulletsTime;
    [Space]
    public List<ParticleSetup> DestuctionParticles;

    [Header("Edge Resposition")]
    public float EdgeOffsetY;
    public float EdgeRepositionOffsetY;
    [Space]
    public float EdgeOffsetX;
    public float EdgeRepositionOffsetX;

    [Header("Sound")]
    public SO_Sound[] SoundsOnShoot;

}
