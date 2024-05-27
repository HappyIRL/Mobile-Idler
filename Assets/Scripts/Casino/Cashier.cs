
using UnityEngine;

public class Cashier
{
	private readonly Casino casino;
	private PlayerWallet wallet;
	private CombinatoricsHandler combinatoricsHandler;

	private float timer = 0f;
	private float interval = 1f;
	private uint productionRate = 0;
	private float depositAccumulator = 0;

	public Cashier(Casino casino, CombinatoricsHandler combinatoricsHandler, PlayerWallet wallet)
	{
		this.casino = casino;
		this.wallet = wallet;
		this.combinatoricsHandler = combinatoricsHandler;
	}

	public void Tick()
	{
		// Get the production rate at the start of the interval
		if (timer == 0f)
		{
			productionRate = (uint)(casino.GetProductionRate() * combinatoricsHandler.TotalScore);
		}

		timer += Time.deltaTime;

		// Accumulate deposit amount until it's at least 1
		depositAccumulator += productionRate * Time.deltaTime;
		uint depositAmount = (uint)depositAccumulator;
		depositAccumulator -= depositAmount;

		wallet.Deposit(depositAmount);

		if (timer >= interval)
		{
			timer = 0f;
		}
	}
}
