using System;
using System.Collections.Generic;
using UnityEngine;

public class GameRoom
{
	private SerializedVector3 position;
	private GameSlot[] gameSlots;

	public GameRoom(GameRoomData data)
	{
		gameSlots = new GameSlot[data.GameSlotDatas.Length];

		for (int i = 0; i < data.GameSlotDatas.Length; i++)
		{
			GameSlotData slotData = data.GameSlotDatas[i];
			GameSlot gameSlot = new GameSlot(slotData);
			gameSlots[i] = gameSlot;
		}
	}

	public GameRoomData FetchData()
	{
		GameRoomData data = new GameRoomData();

		GameSlotData[] slotDatas = new GameSlotData[gameSlots.Length];

		for (int i = 0; i < gameSlots.Length; i++)
		{
			slotDatas[i] = gameSlots[i].FetchData();
		}

		data.GameSlotDatas = slotDatas;
		data.Position = position;

		return data;
	}
}

[Serializable]
public struct GameRoomData
{
	public SerializedVector3 Position;
	public GameSlotData[] GameSlotDatas;
}
