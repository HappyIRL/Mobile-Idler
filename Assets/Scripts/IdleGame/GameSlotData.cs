using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/GameSlotData")]
public class GameSlotData : ScriptableObject
{
	public GameType GameType => gameType;
	[SerializeField] private GameType gameType;
}
