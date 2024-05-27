using System.Collections.Generic;
using Assets.Scripts.UI;
using Assets.Scripts.Utils;
using CasinoIdler;
using UnityEngine;

public class Selector
{
	private PlayerCamera playerCamera;
	private CasinoUIHandler casinoUIHandler;
	private PlayerInputBroadcast playerInputBroadcast;

	public Vector2Int SelectedPosition { get; private set; }
	public SelectableUI Selection { get; private set; }
	public SelectableUI OldSelection { get; private set; }

	public System.Action NewSelection;

	public Selector(PlayerCamera playerCamera, CasinoUIHandler casinoUIHandler, PlayerInputBroadcast playerInputBroadcast)
	{
		this.playerInputBroadcast = playerInputBroadcast;
		playerInputBroadcast.Touch0Tap += OnTouch0Tap;
		this.casinoUIHandler = casinoUIHandler;
		this.playerCamera = playerCamera;
	}

	protected void OnTouch0Tap(Vector2 touchPos)
	{
		Vector2 worldPos = playerCamera.ScreenPointToWorldPos(touchPos);
		SelectedPosition = casinoUIHandler.GetCasinoWorldPosition(worldPos);
		Selection = casinoUIHandler.GetCasinoUI(SelectedPosition);

		NewSelection?.Invoke();

		OldSelection = Selection;
	}

	public void UnsubscribeEvents()
	{
		playerInputBroadcast.Touch0Tap -= OnTouch0Tap;
	}
}

public interface ISelectable
{
	public ICollection<IAction> GetActions();
	public string Name { get; }
}

