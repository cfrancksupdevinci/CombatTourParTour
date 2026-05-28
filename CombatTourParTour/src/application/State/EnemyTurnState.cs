public class EnemyTurnState : ICombatState
{
  public string LabelEnemyTurn => "EnemyTurn";

  public void Execute(Waves context)
  {
    Console.WriteLine(LabelEnemyTurn);
    CombatActions.EnemyAttackHero(context.Hero, context.Enemy);

    if (context.IsHeroDead())
    {
      context.SetState(new DefeatState());
      return;
    }

    context.SetState(new HeroTurnState(hero: context.Hero));
  }
}