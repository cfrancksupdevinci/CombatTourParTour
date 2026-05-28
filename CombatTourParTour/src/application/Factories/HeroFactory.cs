public class HeroFactory
{
  private const long WarriorPV = 120;
  private const long MagePV = 80;
  private const long ThiefPV = 90;
  private const long DefaultRemainingCare = 2;

  private static Hero? _hero;

  public static Hero Create(int id, string name, Classes classe)
  {
    _hero = classe switch
    {
      Classes.Warrior => new Hero(
        id: id,
        name: name,
        actual_pv: WarriorPV,
        remaining_care: DefaultRemainingCare,
        classe: classe
      ),
      Classes.Mage => new Hero(
        id: id,
        name: name,
        actual_pv: MagePV,
        remaining_care: DefaultRemainingCare,
        classe: classe
      ),
      Classes.Thief => new Hero(
        id: id,
        name: name,
        actual_pv: ThiefPV,
        remaining_care: DefaultRemainingCare,
        classe: classe
      ),
      _ => throw new ArgumentOutOfRangeException(nameof(classe))
    };
    return _hero;
  }

  public static Hero GetHero()
  {
    return _hero ?? throw new InvalidOperationException("Hero has not been created yet.");
  }
}