
using System;
using System.Collections.Generic;
using Assets.Scripts.UI;
using CasinoIdler;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameFloorUI : SelectableUI
{
	public override ISelectable Selectable => selectable;

	private ISelectable selectable;
	private GameFloor gameFloor;
	private Tilemap tileMap;
	private CasinoSprites casinoSprites;
	private List<SelectableUI>[,] selectableUILists;
	private Tilemap casinomap;

	public void Init(GameFloor gameFloor, Tilemap tileMap, Tilemap casinomap, CasinoSprites casinoSprites, List<SelectableUI>[,] selectableUIs)
	{
		this.casinomap = casinomap;
		this.selectableUILists = selectableUIs;
		this.casinoSprites = casinoSprites;
		this.gameFloor = gameFloor;
		selectable = gameFloor;
		this.tileMap = tileMap;

		RegisterUiField();
		DrawAll();
	}

	private void DrawAll()
	{
		for (int i = 0; i < gameFloor.GameRooms.Count; i++)
		{
			DrawRoom(i);
		}
	}

	public override void OnAction(IAction action)
	{
		if (action.actionType == ActionType.Sell)
		{
			tileMap.ClearAllTiles();
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
		else
		{
			for (int i = 0; i < gameFloor.GameRooms.Count + 1; i++)
			{
				int x = (i % CasinoUIConstants.FLOOR_ROW_SIZE) * CasinoUIConstants.GAMEROOM_SIZE;
				int y = i / CasinoUIConstants.FLOOR_ROW_SIZE * CasinoUIConstants.GAMEROOM_SIZE;

				if (selectableUILists[x, selectableUILists.GetLength(1) - 1 - y].Count < 3)
				{
					DrawRoom(i);
					break;
				}
			}
		}
	}

	private void DrawRoom(int index)
	{
		GameRoomUI gameRoomUI = new GameRoomUI();
		int x = (index % CasinoUIConstants.FLOOR_ROW_SIZE) * CasinoUIConstants.GAMEROOM_SIZE;
		int y = index / CasinoUIConstants.FLOOR_ROW_SIZE * CasinoUIConstants.GAMEROOM_SIZE;
		gameRoomUI.Init(gameFloor.GameRooms[index], tileMap, casinomap, new Vector2Int(x, selectableUILists.GetLength(1) - 1 - y), casinoSprites, selectableUILists);
	}

	protected override void RegisterUiField()
	{
		foreach (List<SelectableUI> selectableUis in selectableUILists)
		{
			selectableUis.Insert(0, this);
		}
	}

	protected override void UnregisterUiField()
	{
		
	}
}
