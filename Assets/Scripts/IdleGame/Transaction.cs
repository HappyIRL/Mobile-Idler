using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transaction
{
	private PlayerWallet playerWallet;

	public Transaction(PlayerWallet playerWallet)
	{
		this.playerWallet = playerWallet;
	}

	/// <summary>
	/// Returns true if the purchase has succeeded.
	/// </summary>
	/// <param name="purchasable"></param>
	/// <returns></returns>
	private bool Purchase(IPurchasable purchasable)
	{
		uint cost = purchasable.GetCost();

		bool enoughFunds = playerWallet.CheckWalletFor(cost);

		if (!enoughFunds)
			return false;

		playerWallet.Withdraw(cost);

		return true;
	}
}
