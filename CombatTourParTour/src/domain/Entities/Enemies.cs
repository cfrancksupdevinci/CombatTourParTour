public class Enemies
{
  public int id;
  public string name;
  public long actual_pv;
  public long pv_max;
  public long damage;

  public Enemies(int id, string name, long actual_pv, long pv_max, long damage)
  {
    this.id = id;
    this.name = name;
    this.actual_pv = actual_pv;
    this.pv_max = pv_max;
    this.damage = damage;
  }
}