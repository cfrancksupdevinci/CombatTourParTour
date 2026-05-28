public class AttackFlow
{
  public Waves Execute(Hero hero)
  {
    var waves = new Waves(hero);
    waves.CombatWon += () => Console.WriteLine("Combat won");
    waves.CombatLost += () => Console.WriteLine("Combat lost");

    while (!waves.CombatIsOver())
    {
      waves.ExecuteState();
    }

    return waves;
  }
}