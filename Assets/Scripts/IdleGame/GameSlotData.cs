using System.Security;
using UnityEngine;

/// <summary>
/// Should be used as asset instance and not as runtime instance
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObjects/GameSlotData")]
public class GameSlotData : ScriptableObject
{
	[SerializeField] private GameType gameType;
	[SerializeField] private uint baseProductionRate = 10;
	[SerializeField] private uint baseCost = 100;

	public GameType GameType => gameType;
	public uint BaseProductionRate => baseProductionRate;
	public uint BaseCost => baseCost;
}
