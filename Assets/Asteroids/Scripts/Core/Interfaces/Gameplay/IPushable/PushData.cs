using UnityEngine;

namespace Asteroids.Core.Interfaces.Models
{
    public sealed class PushData
    {
        public float PushForce;
        public float TorqueForce;
        public Vector2 HitPoint;
        public Vector2 HitDirection;
        public GameObject Source;

        public PushData(float pushForce, float torqueForce, Vector2 hitPoint, Vector2 hitDirection, GameObject source)
        {
            PushForce = pushForce;
            TorqueForce = torqueForce;
            HitPoint = hitPoint;
            HitDirection = hitDirection;
            Source = source;
        }
    }
}