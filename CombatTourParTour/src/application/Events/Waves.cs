public class Waves
{
  private Enemies? enemy;


  //refactor, while enemy1, enemy2, enemy3 are the same, we can create a method that takes the type of enemy as parameter and create the enemy accordingly
  //if actual_pv of enemy1 is 0, then we can call the method to create enemy2 and so on
  private void CreateEnemy(int enemyType)
  {
    if (enemyType == 1)
    {
      enemy = EnemyFactory.CreateStandard(1);
      Console.WriteLine("You encounter an enemy!");
    }
    else if (enemyType == 2)
    {
      Console.WriteLine("The enemy has duplicated itself !");
      for (int i = 2; i <= 2; i++)
      {
        enemy = EnemyFactory.CreateStandard(i);
        Console.WriteLine($"Enemy Name: {enemy.name}");
        Console.WriteLine($"Enemy PV: {enemy.actual_pv}/{enemy.pv_max}");
      }
    }
    else if (enemyType == 3)
    {
      Console.WriteLine("Wait! A final boss?");
      enemy = EnemyFactory.CreateBoss(1);
    }

    if (enemy is null)
    {
      return;
    }

    var enemyName = enemy.name;
    var enemyPV = enemy.actual_pv;
    Console.WriteLine($"Enemy Name: {enemyName}");
    Console.WriteLine($"Enemy PV: {enemyPV}/{enemy.pv_max}");
  }
}