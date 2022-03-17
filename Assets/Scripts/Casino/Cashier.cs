
public class Cashier
{
	private readonly Casino casino;
	private PlayerWallet wallet;

	public Cashier(Casino casino, PlayerWallet wallet)
	{
		this.casino = casino;
	}

	public void OnTick()
	{
		uint depositAmount = casino.GetMoneyLastIdleTick();
		wallet.Deposit(depositAmount);
	}
}
