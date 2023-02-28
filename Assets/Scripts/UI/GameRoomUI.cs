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
	private Tilemap tilemap;
	private Tilemap casinomap;
	private CasinoSprites casinoSprites;
	private List<SelectableUI>[,] selectableUILists;
	private Vector2Int position;

	public void Init(GameRoom gameRoom, Tilemap tileMap, Tilemap casinomap, Vector2Int position, CasinoSprites casinoSprites, List<SelectableUI>[,] selectableUIs)
	{
		this.casinomap = casinomap;
		this.position = position;
		this.selectableUILists = selectableUIs;
		this.tilemap = tileMap;
		this.gameRoom = gameRoom;
		selectable = gameRoom;

		this.casinoSprites = casinoSprites;

		RegisterUiField();
		DrawAll();
	}

	public void DrawAll()
	{
		for (int i = 0; i < CasinoUIConstants.GAMEROOM_SIZE * CasinoUIConstants.GAMEROOM_SIZE; i++)
		{
			int x = i % CasinoUIConstants.GAMEROOM_SIZE + position.x;
			int y = -i / CasinoUIConstants.GAMEROOM_SIZE + position.y;

			for (var j = 0; j < selectableUILists[x, y].Count; j++)
			{
				SelectableUI selectableUI = selectableUILists[x, y][j];
				if (selectableUI is GameSlotUI)
				{
					selectableUILists[x, y].RemoveAt(j);
					tilemap.SetTile(new Vector3Int(x, y, 0), null);
				}
			}
		}

		for (int i = 0; i < gameRoom.GameSlots.Count; i++)
		{
			DrawGameSlot(i);
		}
	}

	public override void OnAction(IAction action)
	{
		if (action.actionType == ActionType.Sell)
		{
			UnregisterUiField();
		}
		else
		{
			for (int i = 0; i < CasinoUIConstants.GAMEROOM_SIZE * CasinoUIConstants.GAMEROOM_SIZE; i++)
			{
				int x = i % CasinoUIConstants.GAMEROOM_SIZE + position.x;
				int y = -i / CasinoUIConstants.GAMEROOM_SIZE + position.y;

				if (selectableUILists[x, y].Count < 4)
				{
					DrawGameSlot(i);
					break;
				}
			}
		}
	}

	private void DrawGameSlot(int index)
	{
		GameSlotUI gameSlotUI = new GameSlotUI();

		int x = index % CasinoUIConstants.GAMEROOM_SIZE + position.x;
		int y = -index / CasinoUIConstants.GAMEROOM_SIZE + position.y;

		gameSlotUI.Init(gameRoom.GameSlots[index], tilemap, new Vector2Int(x, y), casinoSprites, selectableUILists, this);
	}

	protected override void RegisterUiField()
	{
		for (int i = 0; i < CasinoUIConstants.GAMEROOM_SIZE * CasinoUIConstants.GAMEROOM_SIZE; i++)
		{
			int x = i % CasinoUIConstants.GAMEROOM_SIZE + position.x;
			int y = -i / CasinoUIConstants.GAMEROOM_SIZE + position.y;

			Tile tile = ScriptableObject.CreateInstance<Tile>();
			Sprite test = casinoSprites.GetRoomSpriteByType(gameRoom.GameType);
			tile.sprite = test;

			casinomap.SetTile(new Vector3Int(x, y, 0), tile);

			selectableUILists[x, y].Insert(0,this);
		}
	}

	protected override void UnregisterUiField()
	{
		for (int i = 0; i < CasinoUIConstants.GAMEROOM_SIZE * CasinoUIConstants.GAMEROOM_SIZE; i++)
		{
			int x = i % CasinoUIConstants.GAMEROOM_SIZE + position.x;
			int y = -i / CasinoUIConstants.GAMEROOM_SIZE + position.y;

			for (var j = 0; j < selectableUILists[x, y].Count; j++)
			{
				SelectableUI selectableUI = selectableUILists[x, y][j];
				if (selectableUI is GameRoomUI || selectableUI is GameSlotUI)
				{
					selectableUILists[x, y].RemoveAt(j);
					tilemap.SetTile(new Vector3Int(x, y, 0), null);

					Tile tile = ScriptableObject.CreateInstance<Tile>();
					Sprite test = casinoSprites.GetRoomSpriteByType((GameTypes)4);
					tile.sprite = test;

					casinomap.SetTile(new Vector3Int(x, y, 0), tile);
				}
			}
		}
	}
}
