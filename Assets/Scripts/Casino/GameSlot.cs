using System;
using System.Collections.Generic;
using CasinoIdler;

public class GameSlot : ISelectable
{
	public System.Action Unselect { get; set; }
	public IReadOnlyList<ISelectable> SubSelections => null;
	public string Name => $"{type} GameSlot";

	public uint ProductionRate { get; private set; }
	private int upgradeLevel; 
	private uint upgradeCost;

	private List<IAction> actions;
	private readonly GameTypes type;
	private readonly uint maxUpgradeLevel;

	public GameSlot(GameSlotData data)
	{
		type = data.Types;
		upgradeLevel = data.UpgradeLevel;
		upgradeCost = data.UpgradeCost;
		ProductionRate = data.ProductionRate;
		maxUpgradeLevel = data.MaxUpgradeLevel;
	}

	public GameSlotData FetchData()
	{
		GameSlotData data = new GameSlotData
		{
			Types = type,
			UpgradeLevel = this.upgradeLevel,
			UpgradeCost = this.upgradeCost,
			ProductionRate = this.ProductionRate,
			MaxUpgradeLevel = this.maxUpgradeLevel
		};

		return data;
	}

	public void InitActions(IAction[] toAddAction)
	{
		actions = new List<IAction>();
		actions.AddRange(toAddAction);
	}

	public ICollection<IAction> GetActions()
	{
		return actions;
	}

	public void Upgrade()
	{
		upgradeLevel += 1;
		upgradeCost += 5;
		ProductionRate += 5;
	}

	public bool CanUpgrade()
	{
		return upgradeLevel < maxUpgradeLevel;
	}

	public uint GetUpgradeCost()
	{
		return upgradeCost;
	}
}

[Serializable]
public struct GameSlotData
{
	public GameTypes Types;
	public int UpgradeLevel;
	public uint UpgradeCost;
	public uint ProductionRate;
	public uint MaxUpgradeLevel;
}
