using System.Collections.Generic;
using Assets.Scripts.UI;
using CasinoIdler;
using UnityEngine;
using UnityEngine.Tilemaps;
using Vector2 = UnityEngine.Vector2;

public class CasinoUI : SelectableUI
{
	public override ISelectable Selectable => selectable;

	private ISelectable selectable;
	private Tilemap tilemap;
	private CasinoSprites casinoSprites;
	private IReadOnlyList<GameFloor> gameFloors;
	private int currentGameFloor;
	private List<SelectableUI>[,] selectableUILists;
	private Tilemap casinomap;

	public CasinoUI(Casino casino, CasinoSprites casinoSprites, Tilemap tilemap, Tilemap casinomap, List<SelectableUI>[,] selectableUILists)
	{
		this.casinomap = casinomap;
		this.selectableUILists = selectableUILists;
		this.tilemap = tilemap;
		this.casinoSprites = casinoSprites;
		selectable = casino;
		gameFloors = casino.GameFloors;
		currentGameFloor = 0;

		RegisterUiField();
		DrawCasino();
	}

	protected sealed override void RegisterUiField()
	{
		foreach (List<SelectableUI> selectableUis in selectableUILists)
		{
			selectableUis.Add(this);
		}
	}

	public override void OnAction(IAction action)
	{
		DrawCasino();
	}

	protected override void UnregisterUiField()
	{

	}

	private void DrawCasino()
	{
		tilemap.ClearAllTiles();
		if (gameFloors.Count < 1)
			return;
		GameFloor gameFloor = gameFloors[currentGameFloor];
		GameFloorUI gameFloorUI = new GameFloorUI();
		tilemap.ClearAllTiles();
		gameFloorUI.Init(gameFloor, tilemap, casinomap, casinoSprites, selectableUILists);
	}
}

public static class CasinoUIConstants
{
	public const int FLOOR_ROW_SIZE = 9;
	public const int GAMEROOM_SIZE = 2;
	public const int CASINO_OFFSET = 4;
}
