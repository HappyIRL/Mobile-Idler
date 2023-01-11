using System;
using System.Collections.Generic;
using CasinoIdler;
using UnityEngine;


public class Casino : ISelectable
{
	public System.Action Unselect { get; set; }
	public IReadOnlyList<ISelectable> SubSelections => gameFloors;
	public string Name => "Casino";


	private List<GameFloor> gameFloors = new List<GameFloor>();
	private IAction[] actions;
	private uint productionRate;
	private const uint BaseGameFloorCost = 5;

	public Casino()
	{
		CreateGameFloor(GetBaseGameFloorData(true));
		CreateCasinoActions();
	}

	public Casino(CasinoData data)
	{
		foreach (var floorData in data.GameFloorsData)
		{
			CreateGameFloor(floorData);
		}

		CreateCasinoActions();
		RefreshProductionRate();
	}

	private uint GetProductionRate()
	{
		RefreshProductionRate();
		return productionRate;
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

	public uint GetMoneyLastIdleTick()
	{
		return GetProductionRate();
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
		data.ProductionRate = GetProductionRate();

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

	private void CreateGameFloor(GameFloorData data)
	{
		GameFloor gameFloor = new GameFloor(data);

		IAction[] gameFloorActions = { new SellGameFloorAction(this, gameFloor, "Sell GameFloor", 5) };
		gameFloor.InitActions(gameFloorActions);

		gameFloors.Add(gameFloor);
	}

	public void CreateNewGameFloor()
	{
		GameFloorData data = GetBaseGameFloorData(false);

		CreateGameFloor(data);
	}

	private void CreateCasinoActions()
	{
		actions = new[] { new PurchaseGameFloorAction(this, "Buy GameFloor", BaseGameFloorCost) };
	}

	private GameFloorData GetBaseGameFloorData(bool isTutorial)
	{
		GameFloorData data = new GameFloorData
		{
			Cost = BaseGameFloorCost,
			IsTutorialFloor = isTutorial
		};

		return data;
	}
}

[Serializable]
public struct CasinoData
{
	public GameFloorData[] GameFloorsData;
	public uint ProductionRate;
}

public enum GameTypes
{
	Roulette,
	Blackjack,
	SlotMachine
}
