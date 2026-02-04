using Asteroids.Core.Interfaces;
using Asteroids.Core.Interfaces.Models;
using System;
using UnityEngine;

namespace Asteroids.Gameplay.General.OnContact
{
    public class DamageOnContact : MonoBehaviour
    {
        #region Fields and events

        public event Action<DamageInfo> DamageDone;

        [Header("Configuration")]
        private int _damage = 1;

        #endregion

        #region Monobehaviour

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out IDamageable damageable))
            {
                DamageInfo damageInfo = new (_damage, collision.ClosestPoint(transform.position), gameObject);
                damageable.TakeDamage(damageInfo);

                DamageDone?.Invoke(damageInfo);
            }
        }

        #endregion

        #region Initialization

        public void SetData(int value)
        {
            _damage = value;
        }

        #endregion
    }
}