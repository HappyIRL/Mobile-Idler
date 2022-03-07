using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum GameType
{
	Roulette,
	Poker,
	Blackjack
}

public class CasinoTileHandler : MonoBehaviour
{
	[SerializeField] private List<GameSlotData> gameSlotDatas = new List<GameSlotData>();

	public IReadOnlyDictionary<CasinoTile, Vector3> CasinoTiles => casinoTiles;
	private Dictionary<CasinoTile, Vector3> casinoTiles = new Dictionary<CasinoTile, Vector3>();

	private Dictionary<GameType, GameSlotData> dataDictionary = new Dictionary<GameType, GameSlotData>();

	private void Awake()
	{
		foreach (GameSlotData data in gameSlotDatas)
		{
			if(!dataDictionary.ContainsKey(data.GameType))
				dataDictionary.Add(data.GameType, data);
			else
				Debug.LogError("Can't create multiple GameSlotDatas with same GameType.");
		}
	}

	private void CreateCasinoTile(Vector3 position, GameType gameType)
	{
		casinoTiles.Add(new CasinoTile(dataDictionary[gameType]), position);
	}
}