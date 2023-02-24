using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameSlotUI
{
	private Tilemap tileMap;

	public void Init(GameSlot gameSlot, Tilemap tileMap, Vector3Int postion)
	{
		this.tileMap = tileMap;

		DrawGameSlot(postion);
	}

	private void DrawGameSlot(Vector3Int postion)
	{
		Tile tile = ScriptableObject.CreateInstance<Tile>();
		Sprite test = Resources.Load<Sprite>("purple_rug");
		tile.sprite = test;

		tileMap.SetTile(postion, tile);
	}
}
