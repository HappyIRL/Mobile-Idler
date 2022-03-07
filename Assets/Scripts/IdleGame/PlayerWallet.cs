using System.Collections;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

public class PlayerWallet : MonoBehaviour
{
	private uint productionRate;
	private ulong wallet;

	private void OnEnable()
	{
		StartCoroutine(CurrencyUpdate());
	}

	public void AddToProductionRate(int value)
	{
		MathUtils.ModifyUint(value, ref productionRate);
	}

	public void AddToWallet(int value)
	{
		MathUtils.ModifyUlong(value, ref wallet);
	}

	private IEnumerator CurrencyUpdate()
	{
		while (true)
		{
			wallet += productionRate;
			yield return new WaitForSeconds(1f);
		}
	}

	private void OnDisable()
	{
		StopAllCoroutines();
	}
}
