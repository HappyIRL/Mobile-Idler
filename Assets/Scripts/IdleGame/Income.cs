using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class Income : MonoBehaviour
{
	[Zenject.Inject] private PlayerWallet playerWallet;

	public Action CurrencyTick;

	private uint productionRate = 1;

	[Button(enabledMode: EButtonEnableMode.Editor)]
	public void IncreaseProductionRate(uint value = 100)
	{
		productionRate += value;
	}

	public void DecreaseProductionRate(uint value = 100)
	{
		if(productionRate >= value)
			productionRate -= value;
		else
			Debug.LogError("ProductionRate can't be below 0");
	}

	private void OnEnable()
	{
		StartCoroutine(CurrencyUpdate());
	}

	private IEnumerator CurrencyUpdate()
	{
		while (true)
		{
			playerWallet.Deposit(productionRate);
			CurrencyTick?.Invoke();
			yield return new WaitForSeconds(1f);
		}
	}

	private void OnDisable()
	{
		StopAllCoroutines();
	}
}
