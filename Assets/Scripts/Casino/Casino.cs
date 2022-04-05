using System;
using System.Collections.Generic;
using CasinoIdler;


public class Casino : ISelectable
{
	public System.Action Unselect { get; set; }
	public IReadOnlyList<ISelectable> SubSelections => gameRooms;
	public bool CanRemoveGameRoom => gameRooms.Count > 1;
	public string Name => "Casino";

	private List<GameRoom> gameRooms = new List<GameRoom>();
	private IAction[] actions;
	private uint productionRate;

	private const uint BaseGameRoomCost = 5;

	public Casino()
	{
		GameRoomData data = GetBaseGameRoomData();
		data.IsTutorialRoom = true;

		CreateGameRoom(data);
		CreateCasinoActions();
	}

	public Casino(CasinoData data)
	{
		foreach (var roomData in data.GameRoomsData)
		{
			CreateGameRoom(roomData);
		}

		CreateCasinoActions();
		RefreshProductionRate();
	}

	private void RefreshProductionRate()
	{
		uint result = 0;

		foreach (var gameRoom in gameRooms)
		{
			result += gameRoom.ProductionRate;
		}

		productionRate = result;
	}

	public uint GetMoneyLastIdleTick()
	{
		RefreshProductionRate();
		return productionRate;
	}

	public CasinoData FetchData()
	{
		CasinoData data = new CasinoData();

		GameRoomData[] gameRoomsData = new GameRoomData[gameRooms.Count];

		for (int i = 0; i < gameRooms.Count; i++)
		{
			gameRoomsData[i] = gameRooms[i].FetchData();
		}

		data.GameRoomsData = gameRoomsData;
		data.ProductionRate = productionRate;

		return data;
	}

	private void CreateGameRoom(GameRoomData data)
	{
		GameRoom gameRoom = new GameRoom(data);

		IAction[] sellAction = {new SellGameRoomAction(this, gameRoom,"Sell GameRoom") };
		gameRoom.InitActions(sellAction);

		gameRooms.Add(gameRoom);
	}

	private void CreateCasinoActions()
	{
		actions = new IAction[] { new PurchaseGameRoomAction("Buy GameRoom", BaseGameRoomCost, this) };
	}

	public void CreateNewGameRoom(GameTypes type)
	{
		GameRoomData data = GetBaseGameRoomData();
		data.GameType = type;

		CreateGameRoom(data);
	}

	private GameRoomData GetBaseGameRoomData()
	{
		GameRoomData data = new GameRoomData
		{
			Cost = BaseGameRoomCost,
			GameType = GameTypes.Roulette,
			IsTutorialRoom = false
		};

		return data;
	}

	public uint RemoveGameRoom(GameRoom gameRoom)
	{
		gameRooms.Remove(gameRoom);
		gameRoom.Unselect?.Invoke();
		return gameRoom.GameRoomValue;
	}

	public ICollection<IAction> GetActions()
	{
		return actions;
	}
}

[Serializable]
public struct CasinoData
{
	public GameRoomData[] GameRoomsData;
	public uint ProductionRate;
}

public enum GameTypes
{
	Roulette,
	Blackjack,
	SlotMachine
}
