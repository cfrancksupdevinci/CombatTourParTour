public class BasicAttackAction
{
  public long Execute(Hero hero, Enemies enemy)
  {
    var basicAttack = new HeroBasicAttack();
    return basicAttack.Execute(hero, enemy);
  }
}