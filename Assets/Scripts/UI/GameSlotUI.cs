using System.Collections.Generic;
using Assets.Scripts.UI;
using CasinoIdler;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameSlotUI : SelectableUI
{
	public override ISelectable Selectable => selectable;

	private ISelectable selectable;
	private CasinoSprites casinoSprites;
	private Tilemap tilemap;
	private GameSlot gameSlot;
	private List<SelectableUI>[,] selectableUILists;
	private Vector2Int position;

	//R E M O V E THIS
	private GameRoomUI gameRoomUi;

	public void Init(GameSlot gameSlot, Tilemap tileMap, Vector2Int position, CasinoSprites casinoSprites, List<SelectableUI>[,] selectableUIs, GameRoomUI gameroomUI)
	{
		this.position = position;
		this.selectableUILists = selectableUIs;
		this.tilemap = tileMap;
		this.gameSlot = gameSlot;
		selectable = gameSlot;
		this.gameRoomUi = gameroomUI;
		this.casinoSprites = casinoSprites;

		RegisterUiField();
		DrawGameSlot();
	}

	public override void OnAction(IAction action)
	{
		if (action.actionType == ActionType.Sell)
		{
			UnregisterUiField();
		}
	}

	private void DrawGameSlot()
	{
		Tile tile = ScriptableObject.CreateInstance<Tile>();

		Sprite test = casinoSprites.GetSpriteByType(gameSlot.GameType);
		tile.sprite = test;

		tilemap.SetTile(new Vector3Int(position.x, position.y, 0), tile);
	}

	protected override void RegisterUiField()
	{
		selectableUILists[position.x, position.y].Insert(0, this);
	}

	protected override void UnregisterUiField()
	{
		selectableUILists[position.x, position.y].Remove(this);
		tilemap.SetTile(new Vector3Int(position.x, position.y, 0), null);
		gameRoomUi.DrawAll();
	}
}
