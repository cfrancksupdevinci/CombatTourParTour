public class ClassChoice
{
  public static Classes Choose()
  {
    Console.WriteLine("Choose your class:");
    Console.WriteLine("1. Warrior");
    Console.WriteLine("2. Mage");
    Console.WriteLine("3. Thief");

    return Console.ReadLine() switch
    {
      "1" => Classes.Warrior,
      "2" => Classes.Mage,
      "3" => Classes.Thief,
      _ => throw new ArgumentOutOfRangeException(nameof(Classes))
    };
  }
}