public interface IWeapon
{
    void Attack();
    void Upgrade();
    bool CanAttack { get; }
    float Damage { get; }
    float AttackSpeed { get; }
}
