public class MenuFlow
{
  public static Classes Choose()
  {
    Console.WriteLine("Choose your an option:");
    Console.WriteLine("1. Attaque de base");
    Console.WriteLine("2. Éclair");
    Console.WriteLine("3. Se soigner");
    Console.WriteLine("4. Journal");

    //TODO : remove ReadLine, and replace it with a method that will return the user's choice, and then use that choice to determine which action to take.
    return Console.ReadLine() switch
    {
      "1" => BasicAttackAction.BasicAttack,
      "2" => SpecialAttackAction.ClasseChoose,
      "3" => HealthAction.ClasseChoose,
      "4" => JournalAction.ClasseChoose,
      _ => throw new ArgumentOutOfRangeException(nameof(Classes))
    };
  }
}

// déplacer dans commande