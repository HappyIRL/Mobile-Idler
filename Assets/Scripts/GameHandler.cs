using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class GameHandler : MonoBehaviour
{
	[Inject] private OnGUISceneView sceneView;
	[Inject] private PrototypeSelector selector;

	public Action Tick;

	private const float IdleTickDuration = 1f;
	private const string CurrentVersion = "1.0";

	private Casino casino;
	private Cashier cashier;
	private PlayerWallet playerWallet;
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
		Debug.Log(OfflineWorker.GetOfflineGeneratedAmount(lastPlayed, casino.GetProductionRate()));
		walletAmount += (uint)OfflineWorker.GetOfflineGeneratedAmount(lastPlayed, casino.GetProductionRate());
		playerWallet = new PlayerWallet(walletAmount);
		cashier = new Cashier(casino, playerWallet);

		sceneView.Init(playerWallet, selector, casino);
		idleTick ??= StartCoroutine(IdleTick());
	}

	private IEnumerator IdleTick()
	{
		while (gameState == GameState.Running)
		{
			Tick?.Invoke();
			cashier.OnTick();
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