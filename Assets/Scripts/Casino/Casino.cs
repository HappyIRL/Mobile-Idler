using System;
using System.Collections.Generic;
using CasinoIdler;
using UnityEngine;
using Action = System.Action;


public class Casino : ISelectable
{
	public Action InternalStructureChanged { get; set; }
	public Action Unselect { get; set; }
	public string Name => "Casino";
	public IReadOnlyList<ISelectable> SubSelections => gameFloors;
	public IReadOnlyList<GameFloor> GameFloors => gameFloors;


	private List<GameFloor> gameFloors = new List<GameFloor>();
	private List<IAction> actions;
	private uint productionRate;
	private const uint BaseGameFloorCost = 5;

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
		return actions;
	}

	public uint RemoveGameFloor(GameFloor gameFloor)
	{
		gameFloors.Remove(gameFloor);
		gameFloor.Unselect?.Invoke();

		return BaseGameFloorCost;
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

		IAction[] gameFloorActions = {  };//new SellGameFloorAction(this, gameFloor, "Sell GameFloor", 5)
		gameFloor.InitActions(gameFloorActions);
		gameFloor.InternalStructureChanged += OnInternalStructureChanged;

		gameFloors.Add(gameFloor);
	}

	private void CreateCasinoActions()
	{
		actions = new List<IAction> {  }; //new PurchaseGameFloorAction(this, "Buy GameFloor", BaseGameFloorCost)
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
