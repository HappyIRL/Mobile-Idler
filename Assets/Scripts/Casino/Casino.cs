using System;
using System.Collections.Generic;
using CasinoIdler;
using UnityEngine;
using Action = System.Action;


public class Casino : ISelectable
{
	public Action InternalStructureChanged { get; set; }
	public string Name => "Casino";
	public IReadOnlyList<GameFloor> GameFloors => gameFloors;
	public bool CanAddGameFloor => gameFloors.Count < maxGameFloors;

	private List<GameFloor> gameFloors = new List<GameFloor>();
	private List<IAction>[] arrayOfActions;
	private uint productionRate;
	private const uint BaseGameFloorCost = 5000;
	private uint maxGameFloors = 3;

	public Casino()
	{
		CreateGameFloor(GetBaseGameFloorData(), true);
		CreateCasinoActions();
		RefreshProductionRate();
	}

	public Casino(CasinoData data)
	{
		foreach (var floorData in data.GameFloorsData)
		{
			CreateGameFloor(floorData, false);
		}

		CreateCasinoActions();
		RefreshProductionRate();
	}

	public uint GetProductionRate()
	{
		return productionRate;
	}

	public CasinoData FetchData()
	{
		CasinoData data = new CasinoData();

		GameFloorData[] gameFloorsData = new GameFloorData[gameFloors.Count];

		for (int i = 0; i < gameFloorsData.Length; i++)
		{
			gameFloorsData[i] = gameFloors[i].FetchData();
		}

		data.GameFloorsData = gameFloorsData;

		return data;
	}

	public ICollection<IAction> GetActions()
	{
		return arrayOfActions[gameFloors.Count - 1];
	}

	public uint RemoveGameFloor(GameFloor gameFloor)
	{
		gameFloors.Remove(gameFloor);

		OnInternalStructureChanged();

		uint refundValue = 0;

		foreach(GameRoom gameRoom in gameFloor.GameRooms)
		{
			if (gameRoom != null)
				refundValue += gameFloor.RemoveGameRoom(gameRoom);
		}

		return refundValue + BaseGameFloorCost;
	}

	public void CreateNewGameFloor()
	{
		GameFloorData data = GetBaseGameFloorData();

		CreateGameFloor(data, false);
	}

	private void RefreshProductionRate()
	{
		uint result = 0;

		foreach (var gameFloor in gameFloors)
		{
			result += gameFloor.GetProductionRate();
		}

		productionRate = result;
	}

	private void CreateGameFloor(GameFloorData data, bool isTutorial)
	{
		GameFloor gameFloor = new GameFloor(data, isTutorial);

		//new SellGameFloorAction(this, gameFloor, "Sell GameFloor", 5)
		IAction[] gameFloorActions = {};
		gameFloor.InitActions(gameFloorActions);
		gameFloor.InternalStructureChanged += OnInternalStructureChanged;

		gameFloors.Add(gameFloor);
	}

	private void CreateCasinoActions()
	{
		arrayOfActions = new List<IAction>[maxGameFloors];

		for (int i = 0; i < maxGameFloors; i++)
		{
			arrayOfActions[i] = new List<IAction> { new PurchaseGameFloorAction(this, "Buy GameFloor", (uint)(BaseGameFloorCost * Mathf.Pow(i + 1, 10))) };
		}
	}

	private GameFloorData GetBaseGameFloorData()
	{
		GameFloorData data = new GameFloorData
		{
			GameRoomsData = null
		};

		return data;
	}

	private void OnInternalStructureChanged()
	{
		RefreshProductionRate();
		InternalStructureChanged?.Invoke();
	}
}

[Serializable]
public struct CasinoData
{
	public GameFloorData[] GameFloorsData;
}

public enum GameTypes
{
	Roulette,
	Blackjack,
	SlotMachine
}
