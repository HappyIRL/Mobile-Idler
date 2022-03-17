using UnityEngine;

namespace CasinoIdler
{
	public abstract class Action
	{
		public abstract string name { get; }

		public abstract bool CanExecute(PlayerWallet wallet);

		public abstract void Execute(PlayerWallet wallet);
	}

	public class CharityAction : Action
	{
		public override string name => $"Donate {amount}$";
		private uint amount;

		public CharityAction(uint amount)
		{
			this.amount = amount;
		}

		public override bool CanExecute(PlayerWallet wallet)
		{
			return wallet.CheckWalletFor(amount);
		}

		public override void Execute(PlayerWallet wallet)
		{
			wallet.Withdraw(amount);
			Debug.Log($"Donated {amount}$ to charity");
		}
	}

	public class DummySelectable : ISelectable
	{
		public CasinoIdler.Action[] GetActions()
		{
			return new CasinoIdler.Action[] { new CasinoIdler.CharityAction(5), new CasinoIdler.CharityAction(1), new CasinoIdler.CharityAction(2) };
		}
	}

	public struct ActionDisplayData
	{
		public string name;
		public bool isDisplayable;
	}
}

