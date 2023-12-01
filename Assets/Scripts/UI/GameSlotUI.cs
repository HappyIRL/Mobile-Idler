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
	private Tilemap floorMap;
	private GameSlot gameSlot;
	private List<SelectableUI>[,] selectableUILists;
	private Vector2Int position;

	//R E M O V E THIS calls upwards!
	private GameRoomUI gameRoomUi;

	public void Init(GameSlot gameSlot, Tilemap floorMap, Vector2Int position, CasinoSprites casinoSprites, List<SelectableUI>[,] selectableUIs, GameRoomUI gameroomUI)
	{
		this.position = position;
		this.selectableUILists = selectableUIs;
		this.floorMap = floorMap;
		this.gameSlot = gameSlot;
		selectable = gameSlot;
		this.gameRoomUi = gameroomUI;
		this.casinoSprites = casinoSprites;

		RegisterUiField();
		DrawGameSlot();
	}

	public override void OnAction(ActionType actionType, Vector2Int pos)
	{
		if (actionType == ActionType.Sell)
		{
			UnregisterUIFields();
		}
	}

	private void DrawGameSlot()
	{
		Tile tile = ScriptableObject.CreateInstance<Tile>();

		Sprite test = casinoSprites.GetSpriteByType(gameSlot.GameType);
		tile.sprite = test;

		floorMap.SetTile(new Vector3Int(position.x, position.y, 0), tile);
	}

	protected override void RegisterUiField()
	{
		selectableUILists[position.x, position.y].Insert(0, this);
	}

	protected override void UnregisterUIFields()
	{
		selectableUILists[position.x, position.y].Remove(this);
		floorMap.SetTile(new Vector3Int(position.x, position.y, 0), null);
		gameRoomUi.DrawAll();
	}
}
