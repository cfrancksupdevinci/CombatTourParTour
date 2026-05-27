public class EnemyFactory
{
  public static Enemies CreateStandard(int id)
  {
    return new Enemies(id, "Standard Enemy", 30, 30, 3);
  }

  public static Enemies CreateBoss(int id)
  {
    return new Enemies(id, "Boss", 100, 100, 5);
  }
}