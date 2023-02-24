using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameFloorUI
{
	private GameFloor gameFloor;
	private Tilemap tileMap;

	public void Init(GameFloor gameFloor, Tilemap tileMap)
	{
		this.gameFloor = gameFloor;
		this.tileMap = tileMap;
		DrawRooms();
	}

	private void DrawRooms()
	{
		for (int i = 0; i < gameFloor.GameRooms.Count; i++)
		{
			GameRoomUI gameRoomUI = new GameRoomUI();
			int x = (i % CasinoUIConstants.FLOOR_ROW_SIZE) * CasinoUIConstants.GAMEROOM_SIZE;
			int y = -i / CasinoUIConstants.FLOOR_ROW_SIZE * CasinoUIConstants.GAMEROOM_SIZE;

			gameRoomUI.Init(gameFloor.GameRooms[i], tileMap, new Vector3Int(x, y, 0));
		}
	}
}

public static class CasinoUIConstants
{
	public const int FLOOR_ROW_SIZE = 9;
	public const int GAMEROOM_SIZE = 2;
}
