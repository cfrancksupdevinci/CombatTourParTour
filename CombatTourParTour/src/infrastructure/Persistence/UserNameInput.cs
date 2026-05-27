public class UserNameInput
{
  public static string GetUserName()
  {
    Console.WriteLine("Welcome in CombatTourParTour");
    Console.WriteLine("Please enter your name:");
    string userName = Console.ReadLine() ?? "Hero";
    Console.WriteLine($"Hello, {userName}!");
    return userName;
  }
}