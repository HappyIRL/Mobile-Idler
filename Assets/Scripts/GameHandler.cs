using System;
using System.Collections;
using Assets.Scripts.UI;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

public class GameHandler : MonoBehaviour
{
	[Inject(Id = "roomMap")] private Tilemap roomMap;
	[Inject(Id = "slotMap")] private Tilemap slotMap;

	[Inject] private CasinoSprites casinoSprites;
	[Inject] private FrontendUI frontendUI;
	[Inject] private PlayerCamera playerCamera;
	[Inject] private PlayerInputBroadcast playerInputBroadcast;

	private const string CurrentVersion = "1.0";

	private Casino casino;
	private Cashier cashier;
	private PlayerWallet playerWallet;
	private UIDisplayer uiDisplayer;
	private CombinatoricsHandler combinatoricsHandler;
	private Selector selector;

	public void SetGameData(GameData? gameData)
	{
		if (!(gameData is { Version: CurrentVersion }))
		{
			OnNewSaveGame();
			return;
		}

		CasinoData casinoData = gameData.Value.CasinoData;
		GameInit(new Casino(casinoData), gameData.Value.Wallet, gameData.Value.LastPlayed);
	}

	public GameData GetGameData()
	{
		return FetchGameData();
	}

	private void Update()
	{
		cashier.Tick();
		uiDisplayer.Tick();
	}

	private void OnNewSaveGame()
	{
		GameInit(new Casino(), 0, null);
	}

	private void GameInit(Casino casino, ulong walletAmount, DateTime? lastPlayed)
	{
		ReleaseReferences();

		this.casino = casino;
		uint offlineWallet = (uint)OfflineWorker.GetOfflineGeneratedAmount(lastPlayed, casino.GetProductionRate());
		walletAmount += offlineWallet;
		playerWallet = new PlayerWallet(walletAmount);
		CasinoUIHandler casinoUIHandler = new CasinoUIHandler(casino, casinoSprites, roomMap, slotMap);
		combinatoricsHandler = new CombinatoricsHandler(casino);
		cashier = new Cashier(casino, combinatoricsHandler, playerWallet);
		selector = new Selector(playerCamera, casinoUIHandler, playerInputBroadcast);
		uiDisplayer = new UIDisplayer(selector, playerWallet, frontendUI, casinoSprites, offlineWallet);
	}

	private void ReleaseReferences()
	{
		selector?.UnsubscribeEvents();
		combinatoricsHandler?.UnsubscribeEvents();
		uiDisplayer?.UnsubscribeEvents();
	}

	private GameData FetchGameData()
	{
		GameData data = new GameData
		{
			CasinoData = casino.FetchData(),
			Wallet = playerWallet.Wallet,
			Version = CurrentVersion,
			LastPlayed = DateTime.Now
		};

		return data;
	}
}

[Serializable]
public struct GameData
{
	public CasinoData CasinoData;
	public string Version;
	public ulong Wallet;
	public DateTime? LastPlayed;
}