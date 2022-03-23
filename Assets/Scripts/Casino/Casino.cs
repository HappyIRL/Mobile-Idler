using System;
using System.Collections.Generic;
using CasinoIdler;
using Action = CasinoIdler.Action;

public class Casino : ISelectable
{
	private List<GameRoom> gameRooms = new List<GameRoom>();
	private uint productionRate;

	private const uint BaseGameRoomCost = 5;

	public Casino()
	{
		CreateGameRoom(null);
	}

	public Casino(CasinoData data)
	{
		foreach (var roomData in data.GameRoomsData)
		{
			CreateGameRoom(roomData);
		}

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

	private void CreateGameRoom(GameRoomData? data)
	{
		GameRoom gameRoom;

		if (data != null)
			gameRoom = new GameRoom(data.Value);
		else
			gameRoom = new GameRoom(GetBaseGameRoomData());

		gameRooms.Add(gameRoom);
	}

	private void OnNewGameRoom()
	{
		CreateGameRoom(null);
	}

	private GameRoomData GetBaseGameRoomData()
	{
		GameRoomData data = new GameRoomData();
		data.Cost = BaseGameRoomCost;

		return data;
	}

	public Action[] GetActions()
	{
		return new Action[] { new PurchaseAction("Add GameRoom", BaseGameRoomCost, OnNewGameRoom) };
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