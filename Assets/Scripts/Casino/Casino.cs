using System;
using JetBrains.Annotations;

public class Casino
{
	private GameRoom[] gameRooms;
	private uint productionRate;

	public Casino()
	{
		NewCasino();
	}

	public Casino(CasinoData data)
	{
		productionRate = data.ProductionRate;

		gameRooms = new GameRoom[data.GameRoomDatas.Length];

		for (var i = 0; i < data.GameRoomDatas.Length; i++)
		{
			var roomData = data.GameRoomDatas[i];
			GameRoom room = new GameRoom(roomData);
			gameRooms[i] = room;
		}
	}

	public uint GetMoneyLastIdleTick()
	{
		return productionRate;
	}

	public CasinoData FetchData()
	{
		CasinoData data = new CasinoData();

		GameRoomData[] roomDatas = new GameRoomData[gameRooms.Length];

		for (int i = 0; i < gameRooms.Length; i++)
		{
			roomDatas[i] = gameRooms[i].FetchData();
		}

		data.GameRoomDatas = roomDatas;
		data.ProductionRate = productionRate;

		return data;
	}

	private void NewCasino()
	{

	}
}

[Serializable]
public struct CasinoData
{
	public GameRoomData[] GameRoomDatas;
	public uint ProductionRate;
}
