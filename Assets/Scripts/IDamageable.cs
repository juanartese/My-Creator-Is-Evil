public interface IDamageable
{
    // This is a contract. Any script that uses this interface 
    // MUST have a TakeDamage method inside it.
    void TakeDamage(int damageAmount);
}