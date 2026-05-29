
public class Waves
{
  private int waveNumber;
  private bool combatIsOver;
  private ICombatState currentState;
  private Enemies enemy = null!;
  private readonly Hero hero;
  private readonly List<string> journal = new();
  public event Action? CombatWon;
  public event Action? CombatLost;
  public Hero Hero => hero;
  public Enemies Enemy => enemy;
  public List<string> Journal => journal;

  public Waves(Hero hero)
  {
    this.hero = hero;
    waveNumber = 1;
    CreateEnemy(waveNumber);
    currentState = new HeroTurnState(hero);
  }

  public void ExecuteState()
  {
    if (combatIsOver)
    {
      return;
    }

    currentState.Execute(this);
  }

  public void RunUntilFinished()
  {
    while (!combatIsOver)
    {
      ExecuteState();
    }
  }

  public void SetState(ICombatState state)
  {
    currentState = state;
  }

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

  public void CompleteCombat(bool isVictory)
  {
    combatIsOver = true;

    if (isVictory)
    {
      CombatWon?.Invoke();
      return;
    }

    CombatLost?.Invoke();
  }

  public bool IsHeroDead() => hero.actual_pv <= 0;

  public bool IsEnemyDead() => enemy.actual_pv <= 0;
}