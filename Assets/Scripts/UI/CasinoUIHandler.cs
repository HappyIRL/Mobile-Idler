using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.UI
{
	public class CasinoUIHandler
	{
		private List<SelectableUI>[,] selectableUILists = new List<SelectableUI>[18, 8];

		private readonly SelectableUI rootSelectable;

		public CasinoUIHandler(Casino casino, CasinoSprites casinoSprites, Tilemap tilemap, Tilemap casinomap)
		{
			for (int i = 0; i < selectableUILists.GetLength(0); i++)
			{
				for (int j = 0; j < selectableUILists.GetLength(1); j++)
				{
					selectableUILists[i, j] = new List<SelectableUI>();
				}
			}

			CasinoUI casinoUI = new CasinoUI(casino, casinoSprites, tilemap, casinomap, selectableUILists);
			rootSelectable = casinoUI;
		}

		public SelectableUI GetCasinoWorldUI(Vector2 worldPos)
		{
			Vector2Int flooredWorldPos = Vector2Int.FloorToInt(worldPos);
			Vector2Int position = new Vector2Int(flooredWorldPos.x - CasinoUIConstants.CASINO_OFFSET, flooredWorldPos.y - CasinoUIConstants.CASINO_OFFSET);

			if (IsInsideCasino(position))
			{
				SelectableUI selectable = selectableUILists[position.x, position.y][0];
				return selectable;
			}

			return rootSelectable;
		}

		private bool IsInsideCasino(Vector2 vec)
		{
			if (vec.x >= 0 && vec.x < 18 && vec.y >= 0 && vec.y < 8)
				return true;

			return false;
		}
	}
}
