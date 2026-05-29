namespace MyApplication
{
  class Program
  {
    static void Main(string[] args)
    {

      var flow = new HeroCreationFlow();
      var hero = flow.Execute();
      Console.WriteLine($"Hero created: {hero.name} ({hero.classe})");
      var attackFlow = new AttackFlow();
      var attack = attackFlow.Execute(hero);

    }
  }
}
