public interface ICoolDown
{
  void Execute(Waves context);
}

public class CoolDown : ICoolDown
{
  private readonly Hero hero;

  public CoolDown(Hero hero)
  {
    this.hero = hero;
  }

  public void Execute(Waves context)
  {
    if (context.IsBasicAttackUsed())
    {
      CoolDownBasicAttacks(context);
    }
    if (context.IsSpecialAttackUsed())
    {
      CoolDownSpecialAttacks(context);
    }
  }
  public void CoolDownBasicAttacks(Waves context)
  {
    Console.WriteLine($"{hero.name} has no basic attack cooldown remaining.");
  }

  public void CoolDownSpecialAttacks(Waves context)
  {
    Console.WriteLine($"{hero.name} has no special attack cooldown remaining.");
  }
}