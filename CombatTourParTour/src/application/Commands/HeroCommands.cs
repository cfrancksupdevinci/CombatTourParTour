public enum HeroCommandChoice
{
  BasicAttack = 1,
  SpecialAttack = 2,
  Heal = 3,
  Journal = 4,
}

public static class HeroCommands
{
  private static readonly IReadOnlyDictionary<HeroCommandChoice, Action<Hero, Enemies, List<string>>> CommandMap =
    new Dictionary<HeroCommandChoice, Action<Hero, Enemies, List<string>>>
    {
      [HeroCommandChoice.BasicAttack] = (hero, enemy, journal) => CombatActions.HeroBasicAttack(hero, enemy, journal),
      [HeroCommandChoice.SpecialAttack] = (hero, enemy, journal) => CombatActions.HeroSpecialAttack(hero, enemy, journal),
      [HeroCommandChoice.Heal] = (hero, _, journal) => CombatActions.HeroHeal(hero, journal),
      [HeroCommandChoice.Journal] = (_, _, journal) => CombatActions.ShowJournal(journal),
    };

  public static HeroCommandChoice Choose()
  {
    while (true)
    {
      Console.WriteLine("Choose your an option:");
      Console.WriteLine("1. Basic Attack");
      Console.WriteLine("2. Special Attack");
      Console.WriteLine("3. Heal");
      Console.WriteLine("4. Journal");

      var input = Console.ReadLine();
      if (int.TryParse(input, out int value) && Enum.IsDefined(typeof(HeroCommandChoice), value))
      {
        return (HeroCommandChoice)value;
      }

      Console.WriteLine("Invalid choice. Please enter a number between 1 and 4.");
    }
  }

  public static void Execute(HeroCommandChoice choice, Hero hero, Enemies enemy, List<string> journal)
  {
    if (CommandMap.TryGetValue(choice, out var command))
    {
      command(hero, enemy, journal);
      return;
    }

    throw new ArgumentOutOfRangeException(nameof(choice));
  }
}