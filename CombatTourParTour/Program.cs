namespace MyApplication
{
  class Program
  {
    static void Main(string[] args)
    {

      // TODO:utiliser plutot un Activator avec une liste de personnage pour les CLass (on peut garder enums)
      var flow = new HeroCreationFlow();
      var hero = flow.Execute();
      Console.WriteLine($"Hero created: {hero.name} ({hero.classe})");
      var attackFlow = new AttackFlow();
      var attack = attackFlow.Execute(hero);

    }
  }
}
