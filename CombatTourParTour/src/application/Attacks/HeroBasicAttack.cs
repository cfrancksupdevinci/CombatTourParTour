public class HeroBasicAttack
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
      Classes.Warrior => AttackDamageConstants.WarriorBaseDamage,
      Classes.Mage => AttackDamageConstants.MageBaseDamage,
      Classes.Thief => AttackDamageConstants.ThiefBaseDamage,
      _ => 0,
    };
  }
}