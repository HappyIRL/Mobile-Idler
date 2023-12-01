using System;
using System.Collections.Generic;
using Assets.Scripts.Utils;
using CasinoIdler;
using Action = System.Action;

public class GameRoom : ISelectable
{
	public System.Action Unselect { get; set; }
	public Action InternalStructureChanged { get; set; }
	public bool CanAddGameSlot => gameSlots.Length < maxGameSlots;
	public string Name => $"{gameType} Room";
	public GameTypes GameType => gameType;

	public IReadOnlyTwoDimensionalArray<GameSlot> GameSlots => new ReadOnlyTwoDimensionalArray<GameSlot>(gameSlots);


	private GameSlot[,] gameSlots = new GameSlot[2,2];
	private List<IAction> actions;
	private int maxGameSlots => gameSlots.Length;
	private GameTypes gameType;

	private const uint BaseGameSlotCost = 5;
	private const uint BaseUpgradeGameSlotCost = 5;

	public GameRoom(GameRoomData data)
	{
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

	public void CreateNewGameSlot(int posX, int posY)
	{
		GameSlotData data = GetBaseGameSlotData();
		data.posX = posX;
		data.posY = posY;

		CreateGameSlot(data);
	}

	public ICollection<IAction> GetActions()
	{
		return actions;
	}

	public GameRoomData FetchData()
	{
		GameRoomData data = new GameRoomData();

		int rows = gameSlots.GetLength(0);
		int columns = gameSlots.GetLength(1);

		GameSlotData[,] slotDatas = new GameSlotData[rows, columns];

		for (int i = 0; i < rows; i++)
		{
			for (int j = 0; j < columns; j++)
			{
				if (gameSlots[i, j] != null)
					slotDatas[i, j] = gameSlots[i, j].FetchData();
			}
		}

		data.GameSlotsData = slotDatas;
		data.GameType = gameType;

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
		int rows = gameSlots.GetLength(0);
		int cols = gameSlots.GetLength(1);

		int removeRow = -1;
		int removeCol = -1;

		for (int i = 0; i < rows; i++)
		{
			for (int j = 0; j < cols; j++)
			{
				if (gameSlots[i, j] == gameSlot)
				{
					removeRow = i;
					removeCol = j;
					break;
				}
			}

			if (removeRow != -1)
				break;
		}

		if (removeRow != -1)
		{
			gameSlots[removeRow, removeCol] = null;
		}
		else
		{
			throw new InvalidOperationException("The specified game room was not found in the array.");
		}

		gameSlot.Unselect?.Invoke();

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
	public GameSlotData[,] GameSlotsData;
	public int posX;
	public int posY;
	public GameTypes GameType;
	public bool IsTutorialRoom;
}
