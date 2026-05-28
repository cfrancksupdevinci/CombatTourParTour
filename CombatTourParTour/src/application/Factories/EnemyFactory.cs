public class EnemyFactory
{
  private const string StandardEnemyName = "Standard Enemy";
  private const long StandardEnemyPV = 30;
  private const long StandardEnemyDamage = 3;

  private const string BossEnemyName = "Boss";
  private const long BossEnemyPV = 100;
  private const long BossEnemyDamage = 5;

  public static Enemies CreateStandard(int id)
  {
    return new Enemies(
      id: id,
      name: StandardEnemyName,
      actual_pv: StandardEnemyPV,
      pv_max: StandardEnemyPV,
      damage: StandardEnemyDamage
    );
  }

  public static Enemies CreateBoss(int id)
  {
    return new Enemies(
      id: id,
      name: BossEnemyName,
      actual_pv: BossEnemyPV,
      pv_max: BossEnemyPV,
      damage: BossEnemyDamage
    );
  }

}