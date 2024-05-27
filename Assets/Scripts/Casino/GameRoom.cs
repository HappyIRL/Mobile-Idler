using System;
using System.Collections.Generic;
using Assets.Scripts.Utils;
using CasinoIdler;
using UnityEngine;
using Action = System.Action;

public class GameRoom : ISelectable
{
	public Action InternalStructureChanged { get; set; }
	public bool CanAddGameSlot => gameSlotCount < maxGameSlots;
	public string Name => $"{gameType} Room";
	public GameTypes GameType => gameType;

	public IReadOnlyTwoDimensionalArray<GameSlot> GameSlots => new ReadOnlyTwoDimensionalArray<GameSlot>(gameSlots);


	private GameSlot[,] gameSlots = new GameSlot[2,2];
	private List<IAction> actions;
	private GameTypes gameType;
	private int maxGameSlots => gameSlots.Length;
	private int gameSlotCount = 0;
	private const uint BaseGameSlotCost = 5;
	private const uint BaseUpgradeGameSlotCost = 5;

	private Vector2Int position;

	public GameRoom(GameRoomData data)
	{
		gameType = data.GameType;
		position = new Vector2Int(data.posX, data.posY);

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

	public void CreateNewGameSlot(int floorPosX, int floorPosY)
	{
		GameSlotData data = GetBaseGameSlotData();

		data.posX = floorPosX % 2;
		data.posY = (floorPosY % 2 == 0) ? 1 : 0;

		CreateGameSlot(data);
	}

	public ICollection<IAction> GetActions()
	{
		return actions;
	}

	public GameRoomData FetchData()
	{
		GameRoomData data = new GameRoomData();

		int cols = gameSlots.GetLength(0);
		int rows = gameSlots.GetLength(1);

		List<GameSlotData> gameSlotData = new List<GameSlotData>();

		for (int i = 0; i < cols; i++)
		{
			for (int j = 0; j < rows; j++)
			{
				if (gameSlots[i, j] != null)
					gameSlotData.Add(gameSlots[i, j].FetchData());
			}
		}

		data.GameSlotsData = gameSlotData;
		data.GameType = gameType;
		data.posX = position.x;
		data.posY = position.y;
		data.IsTutorialRoom = false;

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
		int cols = gameSlots.GetLength(0);
		int rows = gameSlots.GetLength(1);

		int removeCol = -1;
		int removeRow = -1;

		for (int i = 0; i < cols; i++)
		{
			for (int j = 0; j < rows; j++)
			{
				if (gameSlots[i, j] == gameSlot)
				{
					removeCol = i;
					removeRow = j;
					break;
				}
			}

			if (removeCol != -1)
				break;
		}

		if (removeCol != -1)
		{
			gameSlots[removeCol, removeRow] = null;
		}
		else
		{
			throw new InvalidOperationException("The specified game room was not found in the array.");
		}

		gameSlotCount--;

		InternalStructureChanged.Invoke();

		return BaseGameSlotCost;
	}

	public uint GetProductionRate()
	{
		uint value = 0;

		for (int i = 0; i < gameSlots.GetLength(0); i++)
		{
			for (int j = 0; j < gameSlots.GetLength(1); j++)
			{
				if(gameSlots[i,j] != null)
					value += gameSlots[i, j].ProductionRate;
			}
		}

		return value;
	}

	private void CreateGameSlot(GameSlotData data)
	{
		GameSlot gameSlot = new GameSlot(data);

		IAction[] gameSlotActions = { new SellGameSlotAction(this, gameSlot, "Sell GameSlot", 5), new GameSlotUpgradeAction(gameSlot, "Upgrade GameSlot", BaseUpgradeGameSlotCost) };
		gameSlot.InitActions(gameSlotActions);
		gameSlot.InternalStructureChanged += OnInternalStructureChanged;

		gameSlots[data.posX, data.posY] = gameSlot;
		gameSlotCount++;

		InternalStructureChanged?.Invoke();
	}

	private GameSlotData GetBaseGameSlotData()
	{
		GameSlotData data = new GameSlotData
		{
			UpgradeLevel = 0,
			UpgradeCost = BaseUpgradeGameSlotCost,
			Types = gameType,
			ProductionRate = 5,
			MaxUpgradeLevel = 10
		};

		return data;
	}

	private void OnInternalStructureChanged()
	{
		InternalStructureChanged?.Invoke();
	}
}

[Serializable]
public struct GameRoomData
{
	public List<GameSlotData> GameSlotsData;
	public int posX;
	public int posY;
	public GameTypes GameType;
	public bool IsTutorialRoom;
}
