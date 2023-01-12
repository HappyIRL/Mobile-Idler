using System;

public class PlayerWallet
{
	private ulong wallet = 100;

	public ulong Wallet => wallet;

	public PlayerWallet(ulong wallet)
	{
		this.wallet = wallet;
	}

	public void Withdraw(uint value)
	{
		wallet -= value;
	}

	public void Deposit(uint value)
	{
		wallet += value;
	}

	public bool CheckWalletFor(uint value)
	{
		if (wallet >= value)
			return true;

		return false;
	}
}
