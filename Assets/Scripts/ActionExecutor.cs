using System;
using CasinoIdler;

public class ActionExecutor
{
	private PrototypeSelector selector;

	private readonly PlayerWallet playerWallet;
	private LastSelectionData lastSelection;
	private ISelectable selection => selector.Selection;
	private CasinoIdler.Action[] selectedActions;

	public ActionExecutor(PlayerWallet playerWallet, PrototypeSelector selector)
	{
		this.playerWallet = playerWallet;
		this.selector = selector;
	}

	public ulong GetWalletAmount()
	{
		return playerWallet.Wallet;
	}

	public ActionDisplayData[] GetActionsDisplayData()
	{
		if (selector.Selection == null)
			return Array.Empty<ActionDisplayData>();

		if (lastSelection.selection == selection)
			return lastSelection.data;

		selectedActions = selection.GetActions();

		ActionDisplayData[] result = new ActionDisplayData[selectedActions.Length];

		for (int i = 0; i < result.Length; i++)
		{
			ActionDisplayData data = new ActionDisplayData();

			data.name = selectedActions[i].Name;
			data.isDisplayable = selectedActions[i].CanExecute(playerWallet);

			result[i] = data;
		}

		lastSelection.selection = selection;
		lastSelection.data = result;

		return result;
	}

	public void ExecuteAction(int index)
	{
		selectedActions[index].Execute(playerWallet);
	}

	public struct LastSelectionData
	{
		public ISelectable selection;
		public ActionDisplayData[] data;
	}
}
