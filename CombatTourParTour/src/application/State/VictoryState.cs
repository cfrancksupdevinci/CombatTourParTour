public class VictoryState : ICombatState
{
  public void Execute(Waves context)
  {
    Console.WriteLine("Victory! All waves are defeated.");
    context.CompleteCombat(isVictory: true);
  }
}