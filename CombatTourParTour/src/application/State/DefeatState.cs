public class DefeatState : ICombatState
{
  public void Execute(Waves context)
  {
    Console.WriteLine("Defeat... The hero has fallen.");
    context.CompleteCombat(isVictory: false);
  }
}