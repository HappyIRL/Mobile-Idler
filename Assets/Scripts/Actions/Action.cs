using UnityEngine;

namespace CasinoIdler
{
	public abstract class Action
	{
		public abstract string Name { get; }

		public abstract bool CanExecute(PlayerWallet wallet);

		public abstract void Execute(PlayerWallet wallet);
	}

	public struct ActionDisplayData
	{
		public string name;
		public bool isDisplayable;
	}

	public class PurchaseAction : Action
	{
		public override string Name { get; }
		private readonly uint cost;
		private System.Action callback;

		public PurchaseAction(string name, uint cost, System.Action callback)
		{
			Name = $"{cost}$ {name}";
			this.cost = cost;
			this.callback = callback;
		}

		public override bool CanExecute(PlayerWallet wallet)
		{
			return wallet.CheckWalletFor(cost);
		}

		public override void Execute(PlayerWallet wallet)
		{
			wallet.Withdraw(cost);
			callback?.Invoke();
		}
	}
}

