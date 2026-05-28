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
    var hero = HeroFactory.GetHero();
    var classe = hero.classe;
    var damage = GetDamage(classe) * 1.5;
    return (long)damage;

    //cooldown 2 tours
  }

  private static long RollMageCritical()
  {
    // dégâts magiques fixes + ignore 50 % de l’armure ennemie, cooldown 3 tours
    var hero = HeroFactory.GetHero();
    var classe = hero.classe;
    var damage = GetDamage(classe);
    //Ignor 50% de l'armure ennemie (à implémenter dans la classe Enemy)
    return (long)damage;
  }

  private static long RollThiefCritical()
  {
    //cooldown 2 tours et 30 % de chance d’infliger des dégâts critiques (dégâts × 2)
    var hero = HeroFactory.GetHero();
    var classe = hero.classe;
    var damage = GetDamage(classe);
    //30% de chance d'infliger des dégâts critiques (dégâts × 2) (à implémenter)
    return (long)damage;
  }
}