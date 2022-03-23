using System;
using System.Data;
using UnityEngine;

public class GameSlot
{
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
}

[Serializable]
public struct GameSlotData
{
	public GameTypes Types;
	public int Level;
	public uint Cost;
}
