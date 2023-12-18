using System.Collections.Generic;
using System.Xml.Serialization;
using Assets.Scripts.UI;
using CasinoIdler;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameSlotUI : SelectableUI
{
	public override ISelectable Selectable => selectable;

	private ISelectable selectable;
	private CasinoSprites casinoSprites;
	private Tilemap slotMap;
	private GameSlot gameSlot;
	private List<SelectableUI>[,] selectableUILists;
	private Vector2Int position;

	public void Init(GameSlot gameSlot, Tilemap slotMap, Vector2Int position, CasinoSprites casinoSprites, List<SelectableUI>[,] selectableUIs)
	{
		this.position = position;
		this.selectableUILists = selectableUIs;
		this.slotMap = slotMap;
		this.gameSlot = gameSlot;
		this.casinoSprites = casinoSprites;
		selectable = gameSlot;

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

		slotMap.SetTile(new Vector3Int(position.x, position.y, 0), tile);
	}

	protected override void RegisterUiField()
	{
		selectableUILists[position.x, CasinoUIConstants.LAST_FLOOR_ROWS_INDEX - position.y].Insert(0, this);
	}

	protected override void UnregisterUIFields()
	{
		selectableUILists[position.x, CasinoUIConstants.LAST_FLOOR_ROWS_INDEX - position.y].Remove(this);
		slotMap.SetTile(new Vector3Int(position.x, position.y, 0), null);
	}
}
