public class VictoryState : ICombatState
{
  public string Name => "Victory";

  public void Execute(Waves context)
  {
    Console.WriteLine("Victory! All waves are defeated.");
    context.NotifyVictory();
    context.EndCombat();
  }
}