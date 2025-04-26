
public interface IDamageable
{
    int MaxHP { get; set; }
    int CurrentHP { get; set; }
    bool Alive { get; set; }

    abstract void TakeDamage(int pDamage);
    abstract void Die();
}
