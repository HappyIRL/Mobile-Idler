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
	private Tilemap roomMap;
	private Tilemap slotMap;
	private CasinoSprites casinoSprites;
	private List<SelectableUI>[,] selectableUILists;

	public void Init(GameFloor gameFloor, Tilemap roomMap, Tilemap slotMap,CasinoSprites casinoSprites, List<SelectableUI>[,] selectableUILists)
	{
		this.selectableUILists = selectableUILists;
		this.gameFloor = gameFloor;
		this.roomMap = roomMap;
		this.slotMap = slotMap;
		this.casinoSprites = casinoSprites;
		selectable = gameFloor;

		RegisterUiField();
		DrawAll();
	}

	private void DrawAll()
	{
		for (int i = 0; i < gameFloor.GameRooms.Columns; i++)
		{
			for (int j = 0; j < gameFloor.GameRooms.Rows; j++)
			{
				if (gameFloor.GameRooms[i, j] != null)
				{
					//conversion from array index to floor position
					Vector2Int floorPos = RoomToFloorPos(i, j);
					DrawRoom(new Vector2Int(floorPos.x, CasinoUIConstants.LAST_FLOOR_ROWS_INDEX - floorPos.y));
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
				DrawRoom(floorPosition);
				break;

			default: 
				throw new InvalidOperationException("Can't resolve ActionType in GameFloorUI");
		}
	}

	private void DrawRoom(Vector2Int floorPosition)
	{
		//snap of room to top left room anchor
		Vector2Int roomAnchorPos = (floorPosition / 2) * 2 + Vector2Int.up;
		GameRoomUI gameRoomUI = new GameRoomUI();
		Vector2Int roomPos = FloorToRoomPos(roomAnchorPos.x, CasinoUIConstants.LAST_FLOOR_ROWS_INDEX - roomAnchorPos.y);
		gameRoomUI.Init(gameFloor.GameRooms[roomPos.x, roomPos.y], roomMap, slotMap, roomAnchorPos, casinoSprites, selectableUILists);
	}

	private Vector2Int RoomToFloorPos(int x, int y)
	{
		return new Vector2Int(x * 2, y * 2);
	}

	private Vector2Int FloorToRoomPos(int x, int y)
	{
		return new Vector2Int(x / 2, y / 2);
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
		roomMap.ClearAllTiles();
		slotMap.ClearAllTiles();

		foreach (var list in selectableUILists)
		{
			for (int i = list.Count - 1; i >= 0; i--)
			{
				if (list[i] is GameFloorUI || list[i] is GameRoomUI || list[i] is GameSlotUI)
				{
					list.RemoveAt(i);
				}
			}
		}
	}
}
