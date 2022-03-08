using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cashier : MonoBehaviour
{
	[Zenject.Inject] private PlayerWallet playerWallet;
	[Zenject.Inject] private Income income;

	private Transaction transaction;
	private ITransactionCallbacks callback;

	private void OnEnable()
	{
		income.CurrencyTick += OnCurrencyTick;
	}

	/// <summary>
	/// Creates a new transaction and callbacks once the transaction is valid.
	/// </summary>
	/// <param name="purchasable"></param>
	/// <param name="callback"></param>
	public void NewTransaction(IPurchasable purchasable, ITransactionCallbacks callback)
	{
		this.callback = callback;
		transaction = new Transaction(playerWallet, purchasable);
	}

	private void OnCurrencyTick()
	{
		if(transaction == null)
			return;

		if(transaction.Validate())
			callback.TransactionIsValid();
	}

	/// <summary>
	/// Returns false if failed.
	/// </summary>
	/// <returns></returns>
	public bool CompleteTransaction()
	{
		return transaction.Pursue();
	}

	private void OnDisable()
	{
		income.CurrencyTick -= OnCurrencyTick;
	}

}
