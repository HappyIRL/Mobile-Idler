using System;
using System.Collections.Generic;
using CasinoIdler;
using Action = System.Action;

public class GameFloor : ISelectable
{
	public string Name => "Floor";
	public System.Action Unselect { get; set; }
	public Action InternalStructureChanged { get; set; }
	public IReadOnlyList<ISelectable> SubSelections => gameRooms;
	public bool CanAddGameRoom => gameRooms.Count < maxGameRooms;
	public IReadOnlyList<GameRoom> GameRooms => gameRooms;

	private List<GameRoom> gameRooms = new List<GameRoom>();
	private List<IAction> actions = new List<IAction>();
	private const uint BaseGameRoomCost = 5;
	private uint maxGameRooms = 36;

	public GameFloor(GameFloorData data, bool isTutorial)
	{
		if (isTutorial)
		{
			CreateGameRoom(GetBaseGameRoomData());
			return;
		}

		if (data.GameRoomsData != null)
		{
			foreach (var roomData in data.GameRoomsData)
			{
				CreateGameRoom(roomData);
			}
		}
	}

	public GameFloorData FetchData()
	{
		GameFloorData data = new GameFloorData();

		GameRoomData[] gameRoomsData = new GameRoomData[gameRooms.Count];

		for (int i = 0; i < gameRooms.Count; i++)
		{
			gameRoomsData[i] = gameRooms[i].FetchData();
		}

		data.GameRoomsData = gameRoomsData;

		return data;
	}

	public void CreateNewGameRoom(GameTypes type)
	{
		GameRoomData data = GetBaseGameRoomData();

		data.GameType = type;
		data.IsTutorialRoom = false;

		CreateGameRoom(data);
	}

	public uint RemoveGameRoom(GameRoom gameRoom)
	{
		gameRooms.Remove(gameRoom);
		gameRoom.Unselect?.Invoke();

		InternalStructureChanged.Invoke();

		return BaseGameRoomCost;
	}

	public ICollection<IAction> GetActions()
	{
		return actions;
	}

	public void InitActions(IAction[] toAddAction)
	{
		actions.AddRange(toAddAction);
		actions.Add(new PurchaseGameRoomAction(this, "Buy GameRoom", BaseGameRoomCost));
	}

	public uint GetProductionRate()
	{
		uint value = 0;

		for (int i = 0; i < gameRooms.Count; i++)
		{
			value += gameRooms[i].GetProductionRate();
		}

		return value;
	}

	private void CreateGameRoom(GameRoomData data)
	{
		GameRoom gameRoom = new GameRoom(data);

		IAction[] gameRoomActions = { new SellGameRoomAction(this, gameRoom, "Sell GameRoom", 5) };
		gameRoom.InitActions(gameRoomActions);
		gameRoom.InternalStructureChanged += OnInternalStructureChanged;

		gameRooms.Add(gameRoom);

		InternalStructureChanged?.Invoke();
	}

	private GameRoomData GetBaseGameRoomData()
	{
		GameRoomData data = new GameRoomData
		{
			GameType = GameTypes.Roulette,
			IsTutorialRoom = true
		};

		return data;
	}

	private void OnInternalStructureChanged()
	{
		InternalStructureChanged?.Invoke();
	}
}

[Serializable]
public struct GameFloorData
{
	public GameRoomData[] GameRoomsData;
}
