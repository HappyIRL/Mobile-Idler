using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

public enum GameType
{
	Roulette,
	Poker,
	Blackjack
}

public class CasinoTileSpawner : MonoBehaviour
{
	[SerializeField] private List<GameSlotData> gameSlotDatas = new List<GameSlotData>();

	private List<CasinoTile> casinoTiles = new List<CasinoTile>();
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
		CasinoTile tile = new CasinoTile(dataDictionary[gameType]);

		casinoTiles.Add(tile);
	}
}