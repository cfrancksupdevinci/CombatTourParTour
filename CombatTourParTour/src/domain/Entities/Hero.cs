public class Hero
{
  public int id;
  public string name;
  public long actual_pv;
  public long remaining_care;
  public Classes classe;

  public Hero(int id, string name, long actual_pv, long remaining_care, Classes classe)
  {
    this.id = id;
    this.name = name;
    this.actual_pv = actual_pv;
    this.remaining_care = remaining_care;
    this.classe = classe;
  }
}