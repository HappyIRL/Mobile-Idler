using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseableRoomUI : MonoBehaviour
{
	private bool hasUI;

	private string gameType;

	private Action selectCallback;
	private Action buyCallback;

	public void OpenNewUI(GameType gameType, Action buyCallback)
	{
		this.gameType = gameType.ToString();

		hasUI = true;

		this.buyCallback = buyCallback;
	}

	private void OnGUI()
	{
		if (!hasUI)
			return;

		GUI.Box(new Rect(10, 10, 100, 90), gameType);

		
	}
}
