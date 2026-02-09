using Asteroids.Core.Interfaces;
using Asteroids.Core.Interfaces.Models;
using System;
using UnityEngine;

namespace Asteroids.Gameplay.General.OnContact
{
    public class PushOnContact : MonoBehaviour
    {
        #region Fields and events

        public event Action<PushData> PushDone;

        [Header("Configuration")]
        private float _pushForce = 0.5f;
        private float _torqueForce = 0.5f;
        private Vector2 _direction;

        #endregion

        #region Initialization

        public void SetData(float pushForce, float torqueForce, Vector2 direction)
        {
            _pushForce = pushForce;
            _torqueForce = torqueForce;
            _direction = direction;
        }

        #endregion

        #region Monobehaviour

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out IPushable pushable))
            {
                PushData pushData = new(_pushForce, _torqueForce, collision.ClosestPoint(transform.position), _direction, gameObject);
                pushable.Push(pushData);

                PushDone?.Invoke(pushData);
            }
        }

        #endregion
    }
}
