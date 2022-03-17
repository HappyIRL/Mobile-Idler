
public class ActionExecutor
{
	[Zenject.Inject] private PrototypeSelector selector;

	private readonly PlayerWallet playerWallet;
	private ISelectable lastSelection;
	private ISelectable selection;
	private CasinoIdler.Action[] selectedActions;

	public ActionExecutor(PlayerWallet playerWallet)
	{
		this.playerWallet = playerWallet;
	}

	public ulong GetWalletAmount()
	{
		return playerWallet.Wallet;
	}

	public void OnSelectionChange(ISelectable selection)
	{
		this.selection = selection;
	}

	public CasinoIdler.ActionDisplayData[] GetActionsDisplayData()
	{
		if (lastSelection != selection)
			selectedActions = selection.GetActions();

		CasinoIdler.ActionDisplayData[] result = new CasinoIdler.ActionDisplayData[selectedActions.Length];

		for (int i = 0; i < result.Length; i++)
		{
			CasinoIdler.ActionDisplayData data = new CasinoIdler.ActionDisplayData();

			data.name = selectedActions[i].name;
			data.isDisplayable = selectedActions[i].CanExecute(playerWallet);

			result[i] = data;
		}

		lastSelection = selection;

		return result;
	}

	public void ExecuteAction(int index)
	{
		selectedActions[index].Execute(playerWallet);
	}
}
