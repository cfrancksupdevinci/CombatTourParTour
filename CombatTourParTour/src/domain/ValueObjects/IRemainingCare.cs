public interface IRemainingCare
{
  void Execute();
}

public class RemainingCare : IRemainingCare
{
  private readonly Hero hero;

  public RemainingCare(Hero hero)
  {
    this.hero = hero;
  }

  public void Execute()
  {
    if (hero.remaining_care <= 0)
    {
      Console.WriteLine($"{hero.name} has no heals remaining.");
      return;
    }

    hero.remaining_care--;
    Console.WriteLine($"{hero.remaining_care} heal(s) remaining for {hero.name}.");
  }
}