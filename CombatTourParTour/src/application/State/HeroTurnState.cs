public class HeroTurnState : ICombatState
{
  private readonly Hero hero;

  public HeroTurnState(Hero hero)
  {
    this.hero = hero;
  }

  public string Name => hero.name + "Turn";

  public void Execute(Waves context)
  {
    Console.WriteLine($"{Name}");
    var choice = HeroCommands.Choose();
    HeroCommands.Execute(choice, context);

    if (context.IsEnemyDead())
    {
      if (context.TryMoveToNextWave())
      {
        context.SetState(new HeroTurnState(hero));
        return;
      }

      context.SetState(new VictoryState());
      return;
    }

    context.SetState(new EnemyTurnState());
  }
}