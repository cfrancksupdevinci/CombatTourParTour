public class HeroFactory
{
  private static Hero _hero;

  public static Hero Create(int id, string name, Classes classe)
  {
    _hero = classe switch
    {
      Classes.Warrior => new Hero(id, name, 120, 2, classe),
      Classes.Mage => new Hero(id, name, 80, 2, classe),
      Classes.Thief => new Hero(id, name, 90, 2, classe),
      _ => throw new ArgumentOutOfRangeException(nameof(classe))
    };
    return _hero;
  }

  public static Hero GetHero()
  {
    return _hero;
  }
}