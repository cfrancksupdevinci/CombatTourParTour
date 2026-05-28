public class EnemyTurnState : ICombatState
{
  public string Name => "EnemyTurn";

  public void Execute(Waves context)
  {
    Console.WriteLine(Name);
    context.EnemyAttackHero();
    context.NotifyEnemyTurnEnded();

    if (context.IsHeroDead())
    {
      context.SetState(new DefeatState());
      return;
    }

    context.SetState(new HeroTurnState(hero: HeroFactory.GetHero()));
  }
}