using System;
using System.Collections;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
	[Zenject.Inject] private OnGUISceneView sceneView;
	[Zenject.Inject] private PrototypeSelector selector;

	public Action Tick;

	private const float IdleTickDuration = 1f;

	private Casino casino;
	private Cashier cashier;
	private PlayerWallet playerWallet;
	private GameState gameState = GameState.Running;

	public void SetGameData(GameData? gameData)
	{
		if (gameData == null)
		{
			OnNewSaveGame();
			return;
		}

		CasinoData casinoData = gameData.Value.CasinoData;
		GameInit(new Casino(casinoData), gameData.Value.Wallet);
	}

	public GameData GetGameData()
	{
		return FetchGameData();
	}

	private void OnNewSaveGame()
	{
		GameInit(new Casino(), 0);
	}

	private void GameInit(Casino casino, ulong walletAmount)
	{
		this.casino = casino;
		playerWallet = new PlayerWallet(walletAmount);
		cashier = new Cashier(casino, playerWallet);
		sceneView.Init(playerWallet, selector, casino);
		StartCoroutine(IdleTick());
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
		GameData data = new GameData();
		data.CasinoData = casino.FetchData();
		data.Wallet = playerWallet.Wallet;
		return data;
	}
}

[Serializable]
public struct GameData
{
	public CasinoData CasinoData;
	public ulong Wallet;
}

public enum GameState
{
	Running
}