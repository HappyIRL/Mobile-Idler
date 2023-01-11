using System;
using System.Collections.Generic;
using CasinoIdler;

public class GameFloor : ISelectable
{
	public string Name => "Floor";
	public System.Action Unselect { get; set; }
	public IReadOnlyList<ISelectable> SubSelections => gameRooms;


	private List<GameRoom> gameRooms = new List<GameRoom>();
	private List<IAction> actions;
	private const uint BaseGameRoomCost = 5;

	public GameFloor(GameFloorData data)
	{


		if (data.IsTutorialFloor)
		{
			CreateGameRoom(GetBaseGameRoomData(true));
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
		GameRoomData data = GetBaseGameRoomData(false);
		data.GameType = type;

		CreateGameRoom(data);
	}

	public uint RemoveGameRoom(GameRoom gameRoom)
	{
		gameRooms.Remove(gameRoom);
		gameRoom.Unselect?.Invoke();

		return BaseGameRoomCost;
	}

	public ICollection<IAction> GetActions()
	{
		return actions;
	}

	public void InitActions(IAction[] toAddAction)
	{
		actions = new List<IAction>();

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
		gameRooms.Add(gameRoom);
	}

	private GameRoomData GetBaseGameRoomData(bool isTutorial)
	{
		GameRoomData data = new GameRoomData
		{
			GameType = GameTypes.Roulette,
			IsTutorialRoom = isTutorial
		};

		return data;
	}
}

[Serializable]
public struct GameFloorData
{
	public GameRoomData[] GameRoomsData;
	public uint Cost;
	public bool IsTutorialFloor;
}
