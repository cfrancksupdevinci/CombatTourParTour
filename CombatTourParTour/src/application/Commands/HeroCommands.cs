public enum HeroCommandChoice
{
  BasicAttack = 1,
  SpecialAttack = 2,
  Heal = 3,
  Journal = 4,
}

public static class HeroCommands
{
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
    switch (choice)
    {
      case HeroCommandChoice.BasicAttack:
        context.HeroBasicAttack();
        break;
      case HeroCommandChoice.SpecialAttack:
        context.HeroSpecialAttack();
        break;
      case HeroCommandChoice.Heal:
        context.HeroHeal();
        break;
      case HeroCommandChoice.Journal:
        context.ShowJournal();
        break;
      default:
        throw new ArgumentOutOfRangeException(nameof(choice));
    }
  }
}