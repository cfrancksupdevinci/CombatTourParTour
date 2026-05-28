public static class CombatActions
{
  public static long HeroBasicAttack(Hero hero, Enemies enemy, List<string> journal)
  {
    var attack = new HeroBasicAttack();
    long damage = attack.Execute(hero, enemy);
    Console.WriteLine($"{hero.name} attacks {enemy.name} and deals {damage} damage.");
    Console.WriteLine($"Enemy HP: {enemy.actual_pv}/{enemy.pv_max}");
    journal.Add($"{hero.name} dealt {damage} damage to {enemy.name}.");
    return damage;
  }

  public static long HeroSpecialAttack(Hero hero, Enemies enemy, List<string> journal)
  {
    var attack = new HeroSpecialAttack();
    long damage = attack.Execute(hero, enemy);
    Console.WriteLine($"{hero.name} uses their special ability on {enemy.name} and deals {damage} damage.");
    Console.WriteLine($"Enemy HP: {enemy.actual_pv}/{enemy.pv_max}");
    journal.Add($"{hero.name} used their special ability on {enemy.name} for {damage} damage.");
    return damage;
  }

  public static long HeroHeal(Hero hero, List<string> journal)
  {
    long maxPv = GetHeroMaxPv(hero.classe);
    long healed = 25;
    long newPv = hero.actual_pv + healed;
    hero.actual_pv = newPv > maxPv ? maxPv : newPv;
    IRemainingCare remainingCare = new RemainingCare(hero);
    remainingCare.Execute();
    Console.WriteLine($"{hero.name} heals for {healed} PV. PV: {hero.actual_pv}/{maxPv}");
    journal.Add($"{hero.name} healed for {healed} PV.");
    return healed;
  }

  public static void ShowJournal(List<string> journal)
  {
    Console.WriteLine("Fight Log:");

    if (journal.Count == 0)
    {
      Console.WriteLine("No events recorded yet.");
      return;
    }

    foreach (var entry in journal.TakeLast(5))
    {
      Console.WriteLine(entry);
    }
  }

  public static long EnemyAttackHero(Hero hero, Enemies enemy)
  {
    long damage = enemy.damage;
    long newPv = hero.actual_pv - damage;
    hero.actual_pv = newPv < 0 ? 0 : newPv;
    Console.WriteLine($"{enemy.name} attacks {hero.name} and deals {damage} damage.");
    Console.WriteLine($"Hero PV: {hero.actual_pv}/{GetHeroMaxPv(hero.classe)}");
    return damage;
  }

  private static long GetHeroMaxPv(Classes classe)
  {
    return classe switch
    {
      Classes.Warrior => 120,
      Classes.Mage => 80,
      Classes.Thief => 90,
      _ => 100,
    };
  }
}