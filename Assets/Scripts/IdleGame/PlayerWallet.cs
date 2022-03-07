using System.Collections;
using NaughtyAttributes;
using UnityEngine;

public class PlayerWallet : MonoBehaviour
{
	private ulong wallet;

	public void Withdraw(uint value)
	{
		wallet -= (uint)value;
	}

	public void Deposit(uint value)
	{
		wallet += (uint)value;

	}

	public bool CheckWalletFor(uint value)
	{
		if (wallet > value)
			return true;

		return false;
	}
}
