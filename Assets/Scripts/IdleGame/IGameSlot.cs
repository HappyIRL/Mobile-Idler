using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameSlot : IPurchasable, IUpgradeable
{
	public GameType GetGameType();
}
