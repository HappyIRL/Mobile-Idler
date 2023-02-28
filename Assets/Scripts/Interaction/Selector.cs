using System.Collections.Generic;
using Assets.Scripts.UI;
using CasinoIdler;
using UnityEngine;

public class Selector
{
	private PlayerCamera playerCamera;
	private CasinoUIHandler casinoUIHandler;

	public SelectableUI Selection { get; private set; }
	public SelectableUI OldSelection { get; private set; }

	public System.Action NewSelection;

	public Selector(PlayerCamera playerCamera, CasinoUIHandler casinoUIHandler, PlayerInputBroadcast playerInputBroadcast)
	{
		playerInputBroadcast.Touch0Tap += OnTouch0Tap;
		this.casinoUIHandler = casinoUIHandler;
		this.playerCamera = playerCamera;

	}

	protected void OnTouch0Tap(Vector2 touchPos)
	{
		Vector2 worldPos = playerCamera.ScreenPointToWorldPos(touchPos);
		Selection = casinoUIHandler.GetCasinoWorldUI(worldPos);

		NewSelection?.Invoke();

		OldSelection = Selection;
	}
}

public interface ISelectable
{
	public ICollection<IAction> GetActions();
	public IReadOnlyList<ISelectable> SubSelections { get; }
	public string Name { get; }
}

