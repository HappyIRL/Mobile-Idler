using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CasinoTile
{
	private GameSlotData slotData;

	private List<GameSlot> gameSlots = new List<GameSlot>();

	public CasinoTile(GameSlotData slotData)
	{
		this.slotData = slotData;
	}

	private void CreateGameSlot()
	{
		GameSlot gameSlot = new GameSlot();
		gameSlot.productionRate = slotData.BaseProductionRate;
	}
}
