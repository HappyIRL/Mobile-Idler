using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct GameSlot : IPurchasable
{
	public GameType type;
	public uint productionRate;
	public uint cost;
	public uint upgradeLevel;
	public uint GetCost() => cost;
}
