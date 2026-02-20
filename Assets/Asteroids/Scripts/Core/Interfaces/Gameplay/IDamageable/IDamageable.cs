using Asteroids.Core.Interfaces.Models;

namespace Asteroids.Core.Interfaces
{
    public interface IDamageable
    {        
        void TakeDamage(DamageData damageData);
        void Die();
    }
}