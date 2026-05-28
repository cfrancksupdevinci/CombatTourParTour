public class BasicAttackAction
{
  public BasicAttackAction()
  {
    BasicAttack basicAttack = new BasicAttack();
    var hero = HeroFactory.GetHero();
    var enemy = EnemyFactory();
    return basicAttack.Execute(hero, enemy);
  }
}