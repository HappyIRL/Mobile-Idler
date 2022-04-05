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


	public uint GameRoomValue { get; private set; }

	public string Name => $"{gameType}Room";

	public uint ProductionRate { get; private set; }

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
			CreateGameSlot(null);
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
		CreateGameSlot(null);
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
		actions.Add(new PurchaseGameSlotAction("Buy GameSlot", BaseGameSlotCost, this));
	}

	public uint RemoveGameSlot(GameSlot gameSlot)
	{
		gameSlots.Remove(gameSlot);
		gameSlot.Unselect?.Invoke();
		ProductionRate -= gameSlot.ProductionRate;
		return gameSlot.GetSellValue();
	}

	private void CreateGameSlot(GameSlotData? data)
	{
		GameSlot gameSlot;

		if (data != null)
		{
			gameSlot = new GameSlot(data.Value);
		}
		else
		{
			gameSlot = new GameSlot(GetBaseGameSlotData());
		}

		ProductionRate += gameSlot.ProductionRate;

		IAction[] sellAction = { new SellGameSlotAction(this, gameSlot, "Sell GameSlot") };
		gameSlot.InitActions(sellAction);

		GameRoomValue += gameSlot.GetSellValue();
		gameSlots.Add(gameSlot);
	}

	private GameSlotData GetBaseGameSlotData()
	{
		GameSlotData data = new GameSlotData
		{
			Level = 1,
			Cost = BaseGameSlotCost,
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
	public uint Cost;
}
