public class Waves
{
  private readonly Hero hero;
  private Enemies enemy = null!;
  private ICombatState currentState;
  private bool combatIsOver;
  private int waveNumber;
  private readonly List<string> journal = new();

  public event Action? CombatWon;
  public event Action? CombatLost;

  public Waves(Hero hero)
  {
    this.hero = hero;
    waveNumber = 1;
    CreateEnemy(waveNumber);
    currentState = new HeroTurnState(hero);
  }
  public bool CombatIsOver()
  {
    return combatIsOver;
  }

  public void ExecuteState()
  {
    if (combatIsOver)
    {
      return;
    }

    currentState.Execute(this);
  }

  public void SetState(ICombatState state)
  {
    currentState = state;
  }

  public void EndCombat()
  {
    combatIsOver = true;
  }

  public void NotifyVictory()
  {
    CombatWon?.Invoke();
  }

  public void NotifyDefeat()
  {
    CombatLost?.Invoke();
  }

  public long HeroBasicAttack()
  {
    var attack = new HeroBasicAttack();
    long damage = attack.Execute(hero, enemy);
    Console.WriteLine($"{hero.name} attacks {enemy.name} and deals {damage} damage.");
    Console.WriteLine($"Enemy HP: {enemy.actual_pv}/{enemy.pv_max}");
    journal.Add($"{hero.name} dealt {damage} damage to {enemy.name}.");
    return damage;
  }

  public long HeroSpecialAttack()
  {
    var attack = new HeroSpecialAttack();
    long damage = attack.Execute(hero, enemy);
    Console.WriteLine($"{hero.name} uses their special ability on {enemy.name} and deals {damage} damage.");
    Console.WriteLine($"Enemy HP: {enemy.actual_pv}/{enemy.pv_max}");
    journal.Add($"{hero.name} used their special ability on {enemy.name} for {damage} damage.");
    return damage;
  }

  public long HeroHeal()
  {
    long maxPv = GetHeroMaxPv(hero.classe);
    long healed = 25;
    long newPv = hero.actual_pv + healed;
    hero.actual_pv = newPv > maxPv ? maxPv : newPv;
    IRemainingCare remainingCare = new RemainingCare(hero);
    remainingCare.Execute(this);
    Console.WriteLine($"{hero.name} heals for {healed} PV. PV: {hero.actual_pv}/{maxPv}");
    journal.Add($"{hero.name} healed for {healed} PV.");
    return healed;
  }

  public void ShowJournal()
  {
    Console.WriteLine("Fight Log:");

    if (journal.Count == 0)
    {
      Console.WriteLine("No events recorded yet.");
      return;
    }

    foreach (var entry in journal.TakeLast(5))
    {
      Console.WriteLine(entry);
    }
  }

  public long EnemyAttackHero()
  {
    long damage = enemy.damage;
    long newPv = hero.actual_pv - damage;
    hero.actual_pv = newPv < 0 ? 0 : newPv;
    Console.WriteLine($"{enemy.name} attacks {hero.name} and deals {damage} damage.");
    Console.WriteLine($"Hero PV: {hero.actual_pv}/{GetHeroMaxPv(hero.classe)}");
    return damage;
  }

  public bool IsHeroDead() => hero.actual_pv <= 0;

  public bool IsEnemyDead() => enemy.actual_pv <= 0;

  public bool TryMoveToNextWave()
  {
    if (waveNumber >= 3)
    {
      return false;
    }

    waveNumber++;
    CreateEnemy(waveNumber);
    return true;
  }

  private void CreateEnemy(int enemyType)
  {
    (enemy, string message) = enemyType switch
    {
      1 => (EnemyFactory.CreateStandard(1), "Wave 1/3: an enemy appears."),
      2 => (EnemyFactory.CreateStandard(2), "Wave 2/3: this enemy was duplicated."),
      _ => (EnemyFactory.CreateBoss(1), "Wave 3/3: the final boss appears."),
    };

    Console.WriteLine(message);

    Console.WriteLine($"Enemy Name: {enemy.name}");
    Console.WriteLine($"Enemy PV: {enemy.actual_pv}/{enemy.pv_max}");
    journal.Add($"A new wave has started: {enemy.name}.");
  }

  private static long GetHeroMaxPv(Classes classe)
  {
    return classe switch
    {
      Classes.Warrior => 120,
      Classes.Mage => 80,
      Classes.Thief => 90,
      _ => 100,
    };
  }
}