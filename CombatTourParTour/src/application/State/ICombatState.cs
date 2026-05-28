public interface ICombatState
{
  string Name { get; }
  void Execute(Waves context);
}