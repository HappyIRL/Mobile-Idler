using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transaction
{
	private PlayerWallet playerWallet;
	private uint cost;

	public Transaction(PlayerWallet playerWallet, IPurchasable purchasable)
	{
		this.playerWallet = playerWallet;
		cost = purchasable.GetCost();
	}

	/// <summary>
	/// Returns true if the transaction is valid.
	/// </summary>
	/// <returns></returns>
	public bool Validate()
	{
		return playerWallet.CheckWalletFor(cost);
	}

	/// <summary>
	/// Returns true if the transaction succeeded.
	/// </summary>
	/// <returns></returns>
	public bool Pursue()
	{
		if (!Validate())
			return false;

		playerWallet.Withdraw(cost);
		return true;
	}
}
