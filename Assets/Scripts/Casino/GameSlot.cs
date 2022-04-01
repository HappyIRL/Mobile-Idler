using System;
using System.Collections.Generic;
using CasinoIdler;

public class GameSlot : ISelectable
{
	public IReadOnlyList<ISelectable> SubSelections => null;
	public string Name => "GameSlot";

	private GameTypes types;
	private int level;
	private uint cost;

	public GameSlot(GameSlotData data)
	{
		types = data.Types;
		level = data.Level;
		cost = data.Cost;
	}

	public GameSlotData FetchData()
	{
		GameSlotData data = new GameSlotData();
		data.Types = types;
		data.Level = level;
		data.Cost = cost;
		return data;
	}

	public ICollection<IAction> GetActions()
	{
		return null;
	}
}

[Serializable]
public struct GameSlotData
{
	public GameTypes Types;
	public int Level;
	public uint Cost;
}
