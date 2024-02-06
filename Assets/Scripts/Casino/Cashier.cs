
public class Cashier
{
	private readonly Casino casino;
	private PlayerWallet wallet;
	private CombinatoricsHandler combinatoricsHandler;

	public Cashier(Casino casino, CombinatoricsHandler combinatoricsHandler, PlayerWallet wallet)
	{
		this.casino = casino;
		this.wallet = wallet;
		this.combinatoricsHandler = combinatoricsHandler;
	}

	public void OnTick()
	{
		uint depositAmount = casino.GetProductionRate() * combinatoricsHandler.TotalScore;
		wallet.Deposit(depositAmount);
	}
}
