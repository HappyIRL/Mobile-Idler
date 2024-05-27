using System;
using System.Collections.Generic;
using Assets.Scripts.Utils;
using CasinoIdler;
using UnityEngine;
using Action = System.Action;

public class GameSlot : ISelectable
{
	public Action InternalStructureChanged { get; set; }
	public string Name => GetName();
	public uint ProductionRate { get; private set; }
	public GameTypes GameType => gameType;

	private uint upgradeLevel; 
	private uint upgradeCost;

	private List<IAction> actions;
	private readonly GameTypes gameType;
	private readonly uint maxUpgradeLevel;
	private Vector2Int position;

	public GameSlot(GameSlotData data)
	{
		gameType = data.Types;
		upgradeLevel = data.UpgradeLevel;
		upgradeCost = data.UpgradeCost;
		ProductionRate = data.ProductionRate;
		maxUpgradeLevel = data.MaxUpgradeLevel;
		position = new Vector2Int(data.posX, data.posY);
	}

	public GameSlotData FetchData()
	{
		GameSlotData data = new GameSlotData
		{
			Types = gameType,
			UpgradeLevel = this.upgradeLevel,
			UpgradeCost = this.upgradeCost,
			ProductionRate = this.ProductionRate,
			MaxUpgradeLevel = this.maxUpgradeLevel,
			posX = position.x,
			posY = position.y
		};

		return data;
	}

	public string GetName()
	{
		switch (gameType)
		{
			case GameTypes.Blackjack:
				return "Blackjack Table";
			case GameTypes.Roulette:
				return "Roulette Table";
			case GameTypes.SlotMachine:
				return "Slot Machine";
			default:
			 throw new ArgumentOutOfRangeException();
		}
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
		ProductionRate += 5;
		upgradeCost = ProductionRate * ProductionRate * ProductionRate;
		InternalStructureChanged?.Invoke();
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
	public int posX;
	public int posY;
	public uint UpgradeLevel;
	public uint UpgradeCost;
	public uint ProductionRate;
	public uint MaxUpgradeLevel;
}
