using System;
using System.Collections.Generic;
using Assets.Scripts.UI;
using CasinoIdler;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameRoomUI : SelectableUI
{
	public override ISelectable Selectable => selectable;

	private ISelectable selectable;
	private GameRoom gameRoom;
	private Tilemap roomMap;
	private Tilemap slotMap;
	private CasinoSprites casinoSprites;
	private List<SelectableUI>[,] selectableUILists;
	private Vector2Int position;

	public void Init(GameRoom gameRoom, Tilemap roomMap, Tilemap slotMap, Vector2Int position, CasinoSprites casinoSprites, List<SelectableUI>[,] selectableUILists)
	{
		this.position = position;
		this.selectableUILists = selectableUILists;
		this.roomMap = roomMap;
		this.slotMap = slotMap;
		this.gameRoom = gameRoom;
		this.casinoSprites = casinoSprites;
		selectable = gameRoom;

		RegisterUiField();
		DrawAll();
	}

	public void DrawAll()
	{

		for (int i = 0; i < gameRoom.GameSlots.Rows; i++)
		{
			for (int j = 0; j < gameRoom.GameSlots.Columns; j++)
			{
				if (gameRoom.GameSlots[i, j] != null)
					DrawGameSlot(new Vector2Int(position.x + i, position.y - j));
			}
		}
	}

	public override void OnAction(ActionType actionType, Vector2Int floorPos)
	{
		switch (actionType)
		{
			case ActionType.Sell:
				UnregisterUIFields();
				break;

			case ActionType.Purchase:
				DrawGameSlot(floorPos);
				break;

			default:
				throw new InvalidOperationException("Can't resolve ActionType in GameFloorUI");
		}
	}

	protected override void RegisterUiField()
	{
		for (int i = 0; i < CasinoUIConstants.GAMEROOM_SIZE * CasinoUIConstants.GAMEROOM_SIZE; i++)
		{
			int x = i % CasinoUIConstants.GAMEROOM_SIZE + position.x;
			int y = -i / CasinoUIConstants.GAMEROOM_SIZE + position.y;

			Tile tile = ScriptableObject.CreateInstance<Tile>();
			Sprite typedSprite = casinoSprites.GetRoomSpriteByType(gameRoom.GameType);
			tile.sprite = typedSprite;

			roomMap.SetTile(new Vector3Int(x, y, 0), tile);

			//conversion to array position from floor position
			selectableUILists[x, CasinoUIConstants.LAST_FLOOR_ROWS_INDEX - y].Insert(0, this);
		}
	}

	protected override void UnregisterUIFields()
	{
		for (int i = 0; i < CasinoUIConstants.GAMEROOM_SIZE * CasinoUIConstants.GAMEROOM_SIZE; i++)
		{
			int x = i % CasinoUIConstants.GAMEROOM_SIZE + position.x;
			int y = -i / CasinoUIConstants.GAMEROOM_SIZE + position.y;

			for (var j = selectableUILists[x, CasinoUIConstants.LAST_FLOOR_ROWS_INDEX - y].Count -1; j >= 0; j--)
			{
				SelectableUI selectableUI = selectableUILists[x, CasinoUIConstants.LAST_FLOOR_ROWS_INDEX - y][j];
				if (selectableUI is GameRoomUI || selectableUI is GameSlotUI)
				{
					//deletes GameSlotUI tile
					selectableUILists[x, CasinoUIConstants.LAST_FLOOR_ROWS_INDEX - y].RemoveAt(j);
					slotMap.SetTile(new Vector3Int(x, y, 0), null);
					roomMap.SetTile(new Vector3Int(x, y, 0), null);
				}
			}
		}
	}

	private void DrawGameSlot(Vector2Int floorPos)
	{
		GameSlotUI gameSlotUI = new GameSlotUI();

		int x = floorPos.x % 2;
		int y = (floorPos.y % 2 == 0) ? 1 : 0;

		gameSlotUI.Init(gameRoom.GameSlots[x, y], slotMap, floorPos, casinoSprites, selectableUILists);
	}
}
