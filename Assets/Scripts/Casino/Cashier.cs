
public class Cashier
{
	private readonly Casino casino;
	private PlayerWallet wallet;

	public Cashier(Casino casino, PlayerWallet wallet)
	{
		this.casino = casino;
		this.wallet = wallet;
	}

	public void OnTick()
	{
		uint depositAmount = casino.GetProductionRate();
		wallet.Deposit(depositAmount);
	}
}
