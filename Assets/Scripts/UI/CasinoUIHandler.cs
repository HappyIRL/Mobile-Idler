using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.UI
{
	public class CasinoUIHandler
	{
		private List<SelectableUI>[,] selectableUILists = new List<SelectableUI>[CasinoUIConstants.FLOOR_COLS, CasinoUIConstants.FLOOR_ROWS];

		private readonly SelectableUI rootSelectable;

		public CasinoUIHandler(Casino casino, CasinoSprites casinoSprites, Tilemap roomMap, Tilemap slotMap)
		{
			for (int i = 0; i < selectableUILists.GetLength(0); i++)
			{
				for (int j = 0; j < selectableUILists.GetLength(1); j++)
				{
					selectableUILists[i, j] = new List<SelectableUI>();
				}
			}

			CasinoUI casinoUI = new CasinoUI(casino, casinoSprites, roomMap, slotMap, selectableUILists);
			rootSelectable = casinoUI;
		}

		public Vector2Int GetCasinoWorldPosition(Vector2 worldPos)
		{
			Vector2Int flooredWorldPos = Vector2Int.FloorToInt(worldPos);
			return new Vector2Int(flooredWorldPos.x - CasinoUIConstants.CASINO_OFFSET, flooredWorldPos.y - CasinoUIConstants.CASINO_OFFSET);
		}

		public SelectableUI GetCasinoUI(Vector2Int casinoPosition)
		{
			if (IsInsideCasino(casinoPosition))
			{
				SelectableUI selectable = selectableUILists[casinoPosition.x, CasinoUIConstants.LAST_FLOOR_ROWS_INDEX - casinoPosition.y][0];
				return selectable;
			}

			return rootSelectable;
		}

		private bool IsInsideCasino(Vector2 vec)
		{
			if (vec.x >= 0 && vec.x < CasinoUIConstants.FLOOR_COLS && vec.y >= 0 && vec.y < CasinoUIConstants.FLOOR_ROWS)
				return true;

			return false;
		}
	}
}
