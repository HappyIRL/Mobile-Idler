using System;
using System.Data;
using UnityEngine;

public enum GameType
{
    Roulette,
    Blackjack,
    SlotMachine
}

public class GameSlot
{
	private GameType type;
	private int level;
	private int cost;

	public GameSlot(GameSlotData data)
	{
		type = data.Type;
		level = data.Level;
		cost = data.Cost;
	}

	public GameSlotData FetchData()
	{
		GameSlotData data = new GameSlotData();
		data.Type = type;
		data.Level = level;
		data.Cost = cost;
		return data;
	}
}

[Serializable]
public struct GameSlotData
{
	public GameType Type;
	public int Level;
	public int Cost;
}
