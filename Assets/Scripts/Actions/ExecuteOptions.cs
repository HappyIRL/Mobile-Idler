using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecuteOptions
{
	public static readonly ExecuteOptions Empty = new ExecuteOptions();

	public ExecuteOptions() { }
}

public class PurchaseExecuteOptions : ExecuteOptions
{
	public PlayerWallet Wallet { get; }

	public PurchaseExecuteOptions(PlayerWallet wallet)
	{
		Wallet = wallet;
	}
}
