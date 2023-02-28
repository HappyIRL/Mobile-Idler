using System;
using System.Collections;
using Assets.Scripts.UI;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

public class GameHandler : MonoBehaviour
{
	[Inject] private OnGUISceneView sceneView;
	[Inject(Id = "floorMap")] private Tilemap floorMap;
	[Inject] private CasinoSprites casinoSprites;
	[Inject] private FrontendUI frontendUI;
	[Inject] private PlayerCamera playerCamera;
	[Inject] private PlayerInputBroadcast playerInputBroadcast;
	[Inject(Id = "casinoMap")] private Tilemap casinomap;

	private const float IdleTickDuration = 1f;
	private const string CurrentVersion = "1.0";

	private Casino casino;
	private Cashier cashier;
	private PlayerWallet playerWallet;
	private UIDisplayer uiDisplayer;
	private GameState gameState = GameState.Running;
	private Coroutine idleTick;

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

	private void OnNewSaveGame()
	{
		GameInit(new Casino(), 0, null);
	}

	private void GameInit(Casino casino, ulong walletAmount, DateTime? lastPlayed)
	{
		this.casino = casino;
		walletAmount += (uint)OfflineWorker.GetOfflineGeneratedAmount(lastPlayed, casino.GetProductionRate());
		playerWallet = new PlayerWallet(walletAmount);
		cashier = new Cashier(casino, playerWallet);

		CasinoUIHandler casinoUIHandler = new CasinoUIHandler(casino, casinoSprites, floorMap, casinomap);
		Selector selector = new Selector(playerCamera, casinoUIHandler, playerInputBroadcast);
		uiDisplayer = new UIDisplayer(selector, playerWallet, frontendUI, casinoSprites);

		sceneView.Init(playerWallet, selector, casino);

		if(idleTick != null)
			StopCoroutine(idleTick);

		idleTick = StartCoroutine(IdleTick());
	}

	private IEnumerator IdleTick()
	{
		while (gameState == GameState.Running)
		{
			cashier.OnTick();
			uiDisplayer.OnTick();
			yield return new WaitForSeconds(IdleTickDuration);
		}
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

public enum GameState
{
	Running
}