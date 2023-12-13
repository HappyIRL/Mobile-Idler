using System.Collections.Generic;
using System.Numerics;
using Assets.Scripts.UI;
using CasinoIdler;
using UnityEngine;
using UnityEngine.Tilemaps;
using Vector2 = UnityEngine.Vector2;

public class CasinoUI : SelectableUI
{
	public override ISelectable Selectable => selectable;

	private ISelectable selectable;
	private Tilemap floorMap;
	private CasinoSprites casinoSprites;
	private IReadOnlyList<GameFloor> gameFloors;
	private int currentGameFloor;
	private List<SelectableUI>[,] selectableUILists;
	private Tilemap casinoMap;

	public CasinoUI(Casino casino, CasinoSprites casinoSprites, Tilemap floorMap, Tilemap casinoMap, List<SelectableUI>[,] selectableUILists)
	{
		this.casinoMap = casinoMap;
		this.selectableUILists = selectableUILists;
		this.floorMap = floorMap;
		this.casinoSprites = casinoSprites;
		selectable = casino;
		gameFloors = casino.GameFloors;
		currentGameFloor = 0;

		RegisterUiField();
		DrawFloor();
	}

	protected sealed override void RegisterUiField()
	{
		foreach (List<SelectableUI> selectableUis in selectableUILists)
		{
			selectableUis.Add(this);
		}
	}

	public override void OnAction(ActionType actionType, Vector2Int pos)
	{
		DrawFloor();
	}

	protected override void UnregisterUIFields()
	{

	}

	private void DrawFloor()
	{
		floorMap.ClearAllTiles();
		if (gameFloors.Count < 1)
			return;
		GameFloor gameFloor = gameFloors[currentGameFloor];
		GameFloorUI gameFloorUI = new GameFloorUI();
		floorMap.ClearAllTiles();
		gameFloorUI.Init(gameFloor, floorMap, casinoMap, casinoSprites, selectableUILists);
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
