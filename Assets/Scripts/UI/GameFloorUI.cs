
using System;
using System.Collections.Generic;
using Assets.Scripts.UI;
using CasinoIdler;
using UnityEngine;
using UnityEngine.Tilemaps;
using Action = CasinoIdler.Action;

public class GameFloorUI : SelectableUI
{
	public override ISelectable Selectable => selectable;

	private ISelectable selectable;
	private GameFloor gameFloor;
	private Tilemap floorMap;
	private CasinoSprites casinoSprites;
	private List<SelectableUI>[,] selectableUILists;
	private Tilemap casinoMap;

	public void Init(GameFloor gameFloor, Tilemap floorMap, Tilemap casinoMap, CasinoSprites casinoSprites, List<SelectableUI>[,] selectableUIs)
	{
		this.casinoMap = casinoMap;
		this.selectableUILists = selectableUIs;
		this.casinoSprites = casinoSprites;
		this.gameFloor = gameFloor;
		selectable = gameFloor;
		this.floorMap = floorMap;

		RegisterUiField();
		DrawAll();
	}

	private void DrawAll()
	{
		for (int i = 0; i < gameFloor.GameRooms.Rows; i++)
		{
			for (int j = 0; j < gameFloor.GameRooms.Columns; j++)
			{
				if (gameFloor.GameRooms[i, j] != null)
				{
					DrawRoom(i, j);
				}
			}
		}
	}

	public override void OnAction(ActionType actionType, Vector2Int floorPosition)
	{
		switch (actionType)
		{
			case ActionType.Sell:
				UnregisterUIFields();
				break;

			case ActionType.Purchase:
				Vector2Int roomPos = floorPosition / 2;
				DrawRoom(roomPos.x, roomPos.y);
				break;

			default: 
				throw new InvalidOperationException("Can't resolve ActionType in GameFloorUI");
		}
	}

	private void DrawRoom(int posX, int posY)
	{
		GameRoomUI gameRoomUI = new GameRoomUI();
		gameRoomUI.Init(gameFloor.GameRooms[posX, posY], floorMap, casinoMap, new Vector2Int(posX * 2,posY * 2), casinoSprites, selectableUILists);
	}

	protected override void RegisterUiField()
	{
		foreach (List<SelectableUI> selectableUis in selectableUILists)
		{
			selectableUis.Insert(0, this);
		}
	}

	protected override void UnregisterUIFields()
	{
		floorMap.ClearAllTiles();
		foreach (var list in selectableUILists)
		{
			for (int i = 0; i < list.Count; i++)
			{
				if (!(list[i] is CasinoUI))
				{
					list.RemoveAt(i);
				}
			}
		}
	}
}
