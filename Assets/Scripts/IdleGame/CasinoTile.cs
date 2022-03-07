using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CasinoTile
{
	private GameSlotData gameType;

	public CasinoTile(GameSlotData gameType)
	{
		this.gameType = gameType;
	}

	private int availableGameSlots = 3;
	private List<ScriptableObject> gameSlots = new List<ScriptableObject>();

	/// <summary>
	/// Tries to create a GameSlotData, returns true if succeeded
	/// </summary>
	/// <returns></returns>
	private bool CreateGameSlot()
	{
		if (availableGameSlots == 0)
			return false;

		gameSlots.Add(new GameSlotData());

		availableGameSlots--;

		return true;
	}
}
