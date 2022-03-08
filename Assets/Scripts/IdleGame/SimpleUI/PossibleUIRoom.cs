
public class PossibleUIRoom : UIRoom
{
	public UIRoomManager manager;

	protected override void OnClick()
	{
		manager.NewRoom(rect, GameType.Roulette);
		Destroy(gameObject);
	}
}
