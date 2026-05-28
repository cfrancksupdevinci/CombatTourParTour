public class DefeatState : ICombatState
{
  public string Name => "Defeat";

  public void Execute(Waves context)
  {
    Console.WriteLine("Loose... The hero has fallen.");
    context.NotifyDefeat();
    context.EndCombat();
  }
}