using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
	[Zenject.Inject] private OnGUISceneView sceneView;
	public Action Tick;

	private const float IDLE_TICK_DURATION = 1f;

	private Casino casino;
	private Cashier cashier;
	private ActionExecutor actionExecutor;
	private PlayerWallet playerWallet;
	private GameState gameState = GameState.Running;

	public void SetGameData(GameData? gameData)
	{
		if (gameData == null)
			OnNewSaveGame();

		//on load implementation
	}

	public GameData GetGameData()
	{
		return FetchGameData();
	}

	private void OnNewSaveGame()
	{
		casino = new Casino();
		GameInit(casino);
	}

	private void GameInit(Casino casino)
	{
		playerWallet = new PlayerWallet();
		cashier = new Cashier(casino, playerWallet);
		actionExecutor = new ActionExecutor(playerWallet);
		sceneView.Init(actionExecutor);
		StartCoroutine(IdleTick());
	}

	private IEnumerator IdleTick()
	{
		while (gameState == GameState.Running)
		{
			Tick?.Invoke();
			cashier.OnTick();
			yield return new WaitForSeconds(IDLE_TICK_DURATION);
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