using UnityEngine;

namespace Asteroids.Core.Interfaces.Models
{
    public sealed class DamageInfo
    {
        public int Amount;
        public Vector2 HitPoint;
        public GameObject Source;

        public DamageInfo(int amount, Vector2 hitPoint, GameObject source)
        {
            Amount = amount;
            HitPoint = hitPoint;
            Source = source;
        }
    }
}