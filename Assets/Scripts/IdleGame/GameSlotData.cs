using System.Security;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/GameSlotData")]
public class GameSlotData : ScriptableObject
{
	[SerializeField] private int productionRate = 10;
	[SerializeField] private GameType gameType;
	[SerializeField] private uint cost = 100;
	
	public uint UpgradeLevel = 1;
	public GameType GameType => gameType;
}
