using System;
using System.Collections.Generic;
using CasinoIdler;
using UnityEngine;
using Action = CasinoIdler.Action;

public class GameRoom : ISelectable
{
	public uint ProductionRate { get; private set; }

	private SerializedVector3 position;
	private List<GameSlot> gameSlots = new List<GameSlot>();
	private GameTypes gameType;

	private const uint BaseGameSlotCost = 5;
	private const uint BaseGameSlotProduction = 5;

	public GameRoom(GameRoomData data)
	{
		position = data.Position;
		gameType = data.GameType;

		if (data.GameSlotsData == null)
		{
			CreateGameSlot(null);
			return;
		}

		foreach (var slotData in data.GameSlotsData)
		{
			CreateGameSlot(slotData);
		}
	}

	public void OnCreateGameSlot()
	{
		CreateGameSlot(null);
	}

	public Action[] GetActions()
	{
		return new Action[] { new PurchaseAction("Add GameSlot", BaseGameSlotCost, OnCreateGameSlot) };
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
			ProductionRate += BaseGameSlotProduction;
		}

		gameSlots.Add(gameSlot);
	}

	private GameSlotData GetBaseGameSlotData()
	{
		GameSlotData data = new GameSlotData();
		data.Level = 1;
		data.Cost = BaseGameSlotCost;
		data.Types = gameType;
		return data;
	}
}

[Serializable]
public struct GameRoomData
{
	public SerializedVector3 Position;
	public GameSlotData[] GameSlotsData;
	public GameTypes GameType;
	public uint Cost;
}
