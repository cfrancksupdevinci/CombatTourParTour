public enum HeroCommandChoice
{
  BasicAttack = 1,
  SpecialAttack = 2,
  Heal = 3,
  Journal = 4,
}

public static class HeroCommands
{
  private static readonly IReadOnlyDictionary<HeroCommandChoice, Action<Waves>> CommandMap =
    new Dictionary<HeroCommandChoice, Action<Waves>>
    {
      [HeroCommandChoice.BasicAttack] = context => context.HeroBasicAttack(),
      [HeroCommandChoice.SpecialAttack] = context => context.HeroSpecialAttack(),
      [HeroCommandChoice.Heal] = context => context.HeroHeal(),
      [HeroCommandChoice.Journal] = context => context.ShowJournal(),
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

  public static void Execute(HeroCommandChoice choice, Waves context)
  {
    if (CommandMap.TryGetValue(choice, out var command))
    {
      command(context);
      return;
    }

    throw new ArgumentOutOfRangeException(nameof(choice));
  }
}