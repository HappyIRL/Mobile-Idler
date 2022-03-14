using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedUiRoom : UiButton, ITransactionCallbacks
{
	[Zenject.Inject] private Cashier cashier;
	[Zenject.Inject] private CloseableRoomUI closeableRoomUi;

	private GameSlotData gameSlotData;

	//Open a transaction and create ui for gameType selection
	protected override void OnClick()
	{
		OpenUi();
	}

	//just horrible, i cant find a better solution
	public void SetGameSlotData(GameSlotData gameSlotData)
	{
		this.gameSlotData = gameSlotData;
	}

	private void OpenUi()
	{
		cashier.NewTransaction(gameSlotData, this);
		//closeableRoomUi.OpenNewUI();
	}

	public void TransactionState(bool valid)
	{
		//enable button to buy slot
	}
}
