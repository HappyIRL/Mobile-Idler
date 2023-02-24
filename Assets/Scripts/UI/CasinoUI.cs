using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CasinoUI
{
	private Tilemap tileMap;

	private Casino casino;
	private IReadOnlyList<GameFloor> gameFloors;
	private int currentGameFloor;

	public void Init(Casino casino)
	{
		this.casino = casino;

		tileMap = GameObject.Find("Floor").GetComponent<Tilemap>();

		gameFloors = casino.GameFloors;
		currentGameFloor = 0;

		DrawFloor();
	}

	private void DrawFloor()
	{
		GameFloor gameFloor = gameFloors[currentGameFloor];
		GameFloorUI gameFloorUI = new GameFloorUI();
		gameFloorUI.Init(gameFloor, tileMap);
	}

	public void SwitchToFloor(int index)
	{
		currentGameFloor = index;
		DrawFloor();
	}
}
