using System;
using System.Collections.Generic;
using CasinoIdler;
using Action = System.Action;

public class GameRoom : ISelectable
{
	public System.Action Unselect { get; set; }
	public IReadOnlyList<ISelectable> SubSelections => gameSlots;
	public Action InternalStructureChanged { get; set; }
	public bool CanAddGameSlot => gameSlots.Count < maxGameSlots;
	public string Name => $"{gameType}Room";
	public IReadOnlyList<GameSlot> GameSlots => gameSlots;


	private readonly List<GameSlot> gameSlots = new List<GameSlot>();
	private List<IAction> actions;
	private uint maxGameSlots = 4;
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

	public void CreateNewGameSlot()
	{
		GameSlotData data = GetBaseGameSlotData();

		CreateGameSlot(data);
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

		InternalStructureChanged.Invoke();

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

		IAction[] gameSlotActions = { new SellGameSlotAction(this, gameSlot, "Sell GameSlot", 5), new GameSlotUpgradeAction(gameSlot, "Upgrade GameSlot", BaseUpgradeGameSlotCost) };
		gameSlot.InitActions(gameSlotActions);
		gameSlot.InternalStructureChanged += OnInternalStructureChanged;

		gameSlots.Add(gameSlot);

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
	public GameSlotData[] GameSlotsData;
	public GameTypes GameType;
	public bool IsTutorialRoom;
}
