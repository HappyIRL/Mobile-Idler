using System;
using System.Collections.Generic;
using CasinoIdler;
using UnityEngine;

public class GameSlot : ISelectable
{
	public System.Action Unselect { get; set; }
	public IReadOnlyList<ISelectable> SubSelections => null;
	public string Name => $"{type} GameSlot";

	public uint ProductionRate { get; }

	private List<IAction> actions;
	private readonly GameTypes type;
	private readonly int level;
	private readonly uint cost;

	public GameSlot(GameSlotData data)
	{
		type = data.Types;
		level = data.Level;
		cost = data.UpgradeCost;
		ProductionRate = data.ProductionRate;
	}

	public GameSlotData FetchData()
	{
		GameSlotData data = new GameSlotData
		{
			Types = type,
			Level = level,
			UpgradeCost = cost,
			ProductionRate = ProductionRate
		};

		return data;
	}

	public void InitActions(IAction[] toAddAction)
	{
		actions = new List<IAction>();
		actions.AddRange(toAddAction);
	}

	public uint GetSellValue()
	{
		return (uint)Mathf.RoundToInt(cost * 0.8f);
	}

	public ICollection<IAction> GetActions()
	{
		return actions;
	}
}

[Serializable]
public struct GameSlotData
{
	public GameTypes Types;
	public int Level;
	public uint UpgradeCost;
	public uint ProductionRate;
}
