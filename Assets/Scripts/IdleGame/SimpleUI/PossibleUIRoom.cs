
public class PossibleUiRoom : UiButton
{
	public UiRoomFactory Factory;

	protected override void OnClick()
	{
		Factory.PlaceNewRoom(rect, GameType.Roulette);
		Destroy(gameObject);
	}
}
