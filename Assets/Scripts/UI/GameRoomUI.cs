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
	private Tilemap floorMap;
	private Tilemap casinoMap;
	private CasinoSprites casinoSprites;
	private List<SelectableUI>[,] selectableUILists;
	private Vector2Int position;

	public void Init(GameRoom gameRoom, Tilemap floorMap, Tilemap casinoMap, Vector2Int position, CasinoSprites casinoSprites, List<SelectableUI>[,] selectableUIs)
	{
		this.casinoMap = casinoMap;
		this.position = position;
		this.selectableUILists = selectableUIs;
		this.floorMap = floorMap;
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
					floorMap.SetTile(new Vector3Int(x, y, 0), null);
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
			UnregisterUIFields();
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

	protected override void RegisterUiField()
	{
		for (int i = 0; i < CasinoUIConstants.GAMEROOM_SIZE * CasinoUIConstants.GAMEROOM_SIZE; i++)
		{
			int x = i % CasinoUIConstants.GAMEROOM_SIZE + position.x;
			int y = -i / CasinoUIConstants.GAMEROOM_SIZE + position.y;

			Tile tile = ScriptableObject.CreateInstance<Tile>();
			Sprite typedSprite = casinoSprites.GetRoomSpriteByType(gameRoom.GameType);
			tile.sprite = typedSprite;

			casinoMap.SetTile(new Vector3Int(x, y, 0), tile);

			selectableUILists[x, y].Insert(0, this);
		}
	}

	protected override void UnregisterUIFields()
	{
		for (int i = 0; i < CasinoUIConstants.GAMEROOM_SIZE * CasinoUIConstants.GAMEROOM_SIZE; i++)
		{
			int x = i % CasinoUIConstants.GAMEROOM_SIZE + position.x;
			int y = -i / CasinoUIConstants.GAMEROOM_SIZE + position.y;

			for (var j = selectableUILists[x, y].Count -1; j >= 0; j--)
			{
				SelectableUI selectableUI = selectableUILists[x, y][j];
				if (selectableUI is GameRoomUI || selectableUI is GameSlotUI)
				{
					selectableUILists[x, y].RemoveAt(j);
					floorMap.SetTile(new Vector3Int(x, y, 0), null);

					Tile tile = ScriptableObject.CreateInstance<Tile>();
					Sprite floorSprite = casinoSprites.GetFloorSprite();
					tile.sprite = floorSprite;
					casinoMap.SetTile(new Vector3Int(x, y, 0), tile);
				}
			}
		}
	}

	private void DrawGameSlot(int index)
	{
		GameSlotUI gameSlotUI = new GameSlotUI();

		int x = index % CasinoUIConstants.GAMEROOM_SIZE + position.x;
		int y = -index / CasinoUIConstants.GAMEROOM_SIZE + position.y;

		gameSlotUI.Init(gameRoom.GameSlots[index], floorMap, new Vector2Int(x, y), casinoSprites, selectableUILists, this);
	}
}
