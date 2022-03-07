using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CasinoTile
{ 
	public Vector3 Position => position;

	private GameSlotData slotData;
	private Vector3 position;

	private int availableGameSlots = 3;
	private List<GameSlotData> gameSlots = new List<GameSlotData>();

	public CasinoTile(GameSlotData slotData, Vector3 position)
	{
		this.slotData = slotData;
		this.position = position;
	}

	/// <summary>
	/// Tries to create a GameSlotData, returns true if succeeded
	/// </summary>
	/// <returns></returns>
	public bool UnluckGameSlot()
	{
		if (availableGameSlots == 0)
			return false;

		gameSlots.Add(new GameSlotData());

		availableGameSlots--;

		return true;
	}

	public GameSlotData GetGameSlotAt(Vector3 position)
	{
		return gameSlots[0];
	}
}
