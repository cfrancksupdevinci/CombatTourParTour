public class HeroCreationFlow
{
  public Hero Execute()
  {
    var userName = UserNameInput.GetUserName();
    var userClass = ClassChoice.Choose();

    return HeroFactory.Create(1, userName, userClass);
  }
}