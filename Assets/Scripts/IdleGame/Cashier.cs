using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TransactionData
{
	public Transaction transaction;
	public int id;
	public ITransactionCallbacks callback;

	public TransactionData(Transaction transaction, int id, ITransactionCallbacks callback)
	{
		this.transaction = transaction;
		this.id = id;
		this.callback = callback;
	}
}

public class Cashier : MonoBehaviour
{
	[Zenject.Inject] private PlayerWallet playerWallet;
	[Zenject.Inject] private Income income;

	private List<TransactionData> transactionDatas = new List<TransactionData>();

	private int id = -1;


	private void OnEnable()
	{
		income.CurrencyTick += OnCurrencyTick;
	}

	/// <summary>
	/// Creates a new transaction and callbacks once the transaction is valid.
	/// </summary>
	/// <param name="purchasable"></param>
	/// <param name="callback"></param>
	public int NewTransaction(IPurchasable purchasable, ITransactionCallbacks callback)
	{
		id++;
		TransactionData data = new TransactionData(new Transaction(playerWallet, purchasable), id, callback);
		transactionDatas.Add(data);
		return id;
	}

	private void OnCurrencyTick()
	{
		if (transactionDatas.Count == 0)
			return;

		foreach (TransactionData data in transactionDatas)
		{
			bool valid = data.transaction.Validate();

			data.callback.TransactionState(valid);
		}

	}

	/// <summary>
	/// Returns false on failure to compelete.
	/// </summary>
	/// <returns></returns>
	public bool CompleteTransaction(int id)
	{
		foreach (TransactionData data in transactionDatas)
		{
			if (data.id == id)
			{
				return data.transaction.Pursue();
			}
		}

		return false;
	}

	private void OnDisable()
	{
		income.CurrencyTick -= OnCurrencyTick;
	}

}
