using System;
using System.Collections.Generic;
using CasinoIdler;
using UnityEngine;

public class GameRoom : ISelectable
{
	public System.Action Unselect { get; set; }
	public IReadOnlyList<ISelectable> SubSelections => gameSlots;
	public bool CanAddGameSlot => gameSlots.Count < maxGameSlots;
	public bool CanRemoveGameSlot => gameSlots.Count > 1;

	public string Name => $"{gameType}Room";

	private readonly SerializedVector3 position;
	private readonly List<GameSlot> gameSlots = new List<GameSlot>();
	private List<IAction> actions;
	private uint maxGameSlots = 4;
	private GameTypes gameType;

	private const uint BaseGameSlotCost = 5;
	private const uint BaseGameSlotProduction = 5;

	public GameRoom(GameRoomData data)
	{
		position = data.Position;
		gameType = data.GameType;

		if (data.IsTutorialRoom)
		{
			CreateGameSlot(GetBaseGameSlotData());
			return;
		}

		if (data.GameSlotsData != null)
		{
			foreach (var slotData in data.GameSlotsData)
			{
				CreateGameSlot(slotData);
			}
		}
	}

	public void CreateNewGameSlot()
	{
		CreateGameSlot(GetBaseGameSlotData());
	}


	public ICollection<IAction> GetActions()
	{
		return actions;
	}

	public GameRoomData FetchData()
	{
		GameRoomData data = new GameRoomData();

		GameSlotData[] slotDatas = new GameSlotData[gameSlots.Count];

		for (int i = 0; i < gameSlots.Count; i++)
		{
			slotDatas[i] = gameSlots[i].FetchData();
		}

		data.GameSlotsData = slotDatas;
		data.Position = position;

		return data;
	}

	public void InitActions(IAction[] toAddAction)
	{
		actions = new List<IAction>();

		actions.AddRange(toAddAction);
		actions.Add(new PurchaseGameSlotAction(this, "Buy GameSlot", BaseGameSlotCost));
	}

	public uint RemoveGameSlot(GameSlot gameSlot)
	{
		gameSlots.Remove(gameSlot);
		gameSlot.Unselect?.Invoke();

		return BaseGameSlotCost;
	}

	public uint GetProductionRate()
	{
		uint value = 0;

		for (int i = 0; i < gameSlots.Count; i++)
		{
			value += gameSlots[i].ProductionRate;
		}

		return value;
	}

	private void CreateGameSlot(GameSlotData data)
	{
		GameSlot gameSlot = new GameSlot(data);

		IAction[] sellAction = { new SellGameSlotAction(this, gameSlot, "Sell GameSlot") };
		gameSlot.InitActions(sellAction);

		gameSlots.Add(gameSlot);
	}

	private GameSlotData GetBaseGameSlotData()
	{
		GameSlotData data = new GameSlotData
		{
			Level = 1,
			UpgradeCost = BaseGameSlotCost,
			Types = gameType,
			ProductionRate = BaseGameSlotProduction
		};

		return data;
	}
}

[Serializable]
public struct GameRoomData
{
	public SerializedVector3 Position;
	public GameSlotData[] GameSlotsData;
	public GameTypes GameType;
	public bool IsTutorialRoom;
}
