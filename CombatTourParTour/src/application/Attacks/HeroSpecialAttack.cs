public class HeroSpecialAttack
{

  public long Execute(Hero hero, Enemies enemy)
  {
    long damage = GetDamage(hero.classe);
    long newPv = enemy.actual_pv - damage;
    enemy.actual_pv = newPv < 0 ? 0 : newPv;
    return damage;
  }

  private static long GetDamage(Classes classe)
  {
    return classe switch
    {
      Classes.Warrior => RollWarriorCritical(),
      Classes.Mage => RollMageCritical(),
      Classes.Thief => RollThiefCritical(),
      _ => 0,
    };
  }

  private static long RollWarriorCritical()
  {
    return (long)(AttackDamageConstants.WarriorBaseDamage * 1.5);
  }

  private static long RollMageCritical()
  {
    return (long)(AttackDamageConstants.MageBaseDamage);
    //Ignore 50% de l'armure ennemie (à implémenter dans la classe Enemy)
  }

  private static long RollThiefCritical()
  {
    bool isCritical = Random.Shared.NextDouble() < 0.30;
    return isCritical ? AttackDamageConstants.ThiefBaseDamage * 2 : AttackDamageConstants.ThiefBaseDamage;
  }
}