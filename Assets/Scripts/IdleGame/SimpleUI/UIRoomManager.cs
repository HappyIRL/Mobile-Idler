using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRoomManager : MonoBehaviour
{
	[Zenject.Inject] private PrefabFactory prefabFactory;
	[Zenject.Inject] private Cashier cashier;

	[SerializeField] private GameObject PlacedUIRoomPrefab;
	[SerializeField] private GameObject PossibleUIRoomPrefab;

	private float buttonSize = 50f;
	private float buttonDistance = 60f;

	private Rect centerRect;

	private Dictionary<Vector2, Rect> freeRects = new Dictionary<Vector2, Rect>();

	private Dictionary<Vector2, PlacedUIRoom> placedUIRooms = new Dictionary<Vector2, PlacedUIRoom>();
	private Dictionary<Vector2, PossibleUIRoom> possibleUIRooms = new Dictionary<Vector2, PossibleUIRoom>();

	//needs to be uneven
	private int rectScaler = 7;

	private void Awake()
	{
		centerRect = new Rect(960f, 540f, buttonSize, buttonSize);

		Vector2 topLeftPosiotion = new Vector2(centerRect.position.x - (rectScaler / 2) * buttonDistance,
			centerRect.position.y - (rectScaler / 2) * buttonDistance);

		for (int i = 0; i < rectScaler; i++)
		{
			for (int j = 0; j < rectScaler; j++)
			{
				Rect rect = new Rect(
					new Vector2(topLeftPosiotion.x + buttonDistance * i, topLeftPosiotion.y + buttonDistance * j),
					centerRect.size);
				freeRects.Add(rect.position, rect);
			}
		}
	}

	private void Start()
	{
		NewRoom(centerRect, GameType.Roulette);
	}

	public void NewRoom(Rect rect, GameType name)
	{
		PlacedUIRoom room = prefabFactory.Create(PlacedUIRoomPrefab, transform).GetComponent<PlacedUIRoom>();
		room.SetDependencies(rect, name.ToString(), ref cashier);
		placedUIRooms.Add(rect.position, room);
		freeRects.Remove(rect.position);
		CreatePossibleRooms(rect.position);
	}

	private void CreatePossibleRooms(Vector2 position)
	{
		List<Vector2> list = new List<Vector2>();
		list.Add(new Vector2(position.x, position.y + buttonDistance));
		list.Add(new Vector2(position.x, position.y - buttonDistance));
		list.Add(new Vector2(position.x + buttonDistance, position.y));
		list.Add(new Vector2(position.x - buttonDistance, position.y));

		foreach (Vector2 item in list)
		{
			if (freeRects.ContainsKey(item) && !possibleUIRooms.ContainsKey(item))
			{
				PossibleUIRoom room = prefabFactory.Create(PossibleUIRoomPrefab, transform).GetComponent<PossibleUIRoom>();
				room.SetDependencies(freeRects[item], "x", ref cashier);
				room.manager = this;
				possibleUIRooms.Add(item, room);
			}
		}
	}
}
