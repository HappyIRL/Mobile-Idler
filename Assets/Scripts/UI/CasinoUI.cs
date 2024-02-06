using System;
using System.Collections.Generic;
using System.Numerics;
using Assets.Scripts.UI;
using CasinoIdler;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CasinoUI : IndexedSelectableUI
{
	public override ISelectable Selectable => selectable;
	public IReadOnlyList<GameFloor> GameFloors => gameFloors;
	public int CurrentGameFloor { get; private set; }

	private ISelectable selectable;
	private Tilemap roomMap;
	private Tilemap slotMap;
	private CasinoSprites casinoSprites;
	private IReadOnlyList<GameFloor> gameFloors;
	private List<SelectableUI>[,] selectableUILists;

	public CasinoUI(Casino casino, CasinoSprites casinoSprites, Tilemap roomMap, Tilemap slotMap, List<SelectableUI>[,] selectableUILists)
	{
		this.selectableUILists = selectableUILists;
		this.roomMap = roomMap;
		this.casinoSprites = casinoSprites;
		this.slotMap = slotMap;
		selectable = casino;
		gameFloors = casino.GameFloors;
		CurrentGameFloor = 0;

		RegisterUiField();
		DrawFloor(CurrentGameFloor);
	}

	protected sealed override void RegisterUiField()
	{
		foreach (List<SelectableUI> selectableUis in selectableUILists)
		{
			selectableUis.Add(this);
		}
	}

	public override void OnAction(ActionType actionType, Vector2Int floorPos)
	{
		switch (actionType)
		{
			case ActionType.Purchase:
				DrawFloor(gameFloors.Count - 1);
				break;

			default:
				throw new InvalidOperationException("Can't resolve ActionType in GameFloorUI");
		}
	}

	public override void OnIndexedAction(int index)
	{
		if (gameFloors.Count <= 1)
			return;

		DrawFloor(index);
	}

	protected override void UnregisterUIFields()
	{
		
	}

	private void DrawFloor(int floorIndex)
	{
		if (gameFloors.Count < 1)
			return;

		roomMap.ClearAllTiles();
		slotMap.ClearAllTiles();

		CurrentGameFloor = floorIndex;
		GameFloorUI gameFloorUI = new GameFloorUI();
		gameFloorUI.Init(gameFloors[CurrentGameFloor], roomMap, slotMap, casinoSprites, selectableUILists);
	}
}

public static class CasinoUIConstants
{
	public const int GAMEROOM_SIZE = 2;
	public const int CASINO_OFFSET = 4;

	//can only ever be a multiple of 2
	public const int FLOOR_COLS = 18;
	public const int FLOOR_ROWS = 8;
	public const int LAST_FLOOR_ROWS_INDEX = FLOOR_ROWS - 1;

}
