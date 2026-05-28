public class Enemy1
{
  private readonly Enemies enemy;

  public Enemy1()
  {
    enemy = EnemyFactory.CreateStandard(1);
    Console.WriteLine("You encounter an enemy!");
    var enemyName = enemy.name;
    var enemyPV = enemy.actual_pv;
    Console.WriteLine($"Enemy Name: {enemyName}");
    Console.WriteLine($"Enemy PV: {enemyPV}/{enemy.pv_max}");
  }
}
