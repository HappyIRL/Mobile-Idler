using System;
using UnityEngine;
using Zenject;

namespace CasinoIdler
{
	public interface IAction
	{
		public abstract string Name { get; }
		public abstract bool CanExecute(PlayerWallet wallet);
	}

	public abstract class Action : IAction
	{
		public virtual string Name { get; protected set; }
		public abstract bool CanExecute(PlayerWallet wallet);
		public abstract void Execute(PlayerWallet wallet);
	}

	public abstract class Action<T> : IAction
	{
		public virtual string Name { get; protected set; }
		public abstract bool CanExecute(PlayerWallet wallet);
		public abstract void Execute(PlayerWallet wallet, T param);
	}

	public class PurchaseGameRoomAction : PurchaseAction<GameTypes>
	{
		private readonly Casino casino;

		public PurchaseGameRoomAction(string name, uint cost, Casino casino) : base(name, cost)
		{
			this.casino = casino;
		}

		protected override void Purchase(GameTypes type)
		{
			casino.CreateNewGameRoom(type);
		}
	}

	public abstract class SellAction : Action
	{
		public sealed override string Name { get; protected set; }

		protected SellAction(string name)
		{
			Name = name;
		}

		protected abstract uint Sell();
		protected abstract bool CanSell();

		public override bool CanExecute(PlayerWallet wallet)
		{
			return CanSell();
		}

		public override void Execute(PlayerWallet wallet)
		{
			wallet.Deposit(Sell());
		}
	}

	public class SellGameRoomAction : SellAction
	{
		private readonly Casino casino;
		private readonly GameRoom gameRoom;

		public SellGameRoomAction(Casino casino, GameRoom gameRoom, string name) : base(name)
		{
			this.casino = casino;
			this.gameRoom = gameRoom;
		}

		protected override uint Sell()
		{
			return casino.RemoveGameRoom(gameRoom);
		}

		protected override bool CanSell()
		{
			return casino.CanRemoveGameRoom;
		}
	}

	public class SellGameSlotAction : SellAction
	{
		private readonly GameRoom gameRoom;
		private readonly GameSlot gameSlot;

		public SellGameSlotAction(GameRoom gameRoom, GameSlot gameSlot, string name) : base(name)
		{
			this.gameRoom = gameRoom;
			this.gameSlot = gameSlot;
		}

		protected override uint Sell()
		{
			return gameRoom.RemoveGameSlot(gameSlot);
		}

		protected override bool CanSell()
		{
			return gameRoom.CanRemoveGameSlot;
		}
	}

	public abstract class PurchaseAction : Action
	{
		public sealed override string Name { get; protected set; }
		private readonly uint cost;

		protected PurchaseAction(string name, uint cost)
		{
			this.cost = cost;
			Name = $"{name}: {cost}$";
		}

		protected abstract void Purchase();

		protected abstract bool CanPurchase();

		public override bool CanExecute(PlayerWallet wallet)
		{
			return CanPurchase() && wallet.CheckWalletFor(cost);
		}

		public override void Execute(PlayerWallet wallet)
		{
			wallet.Withdraw(cost);
			Purchase();
		}
	}

	public class PurchaseGameSlotAction : PurchaseAction
	{
		private readonly GameRoom gameRoom;

		public PurchaseGameSlotAction(string name, uint cost, GameRoom room) : base(name, cost)
		{
			gameRoom = room;
		}

		protected override void Purchase()
		{
			gameRoom.CreateNewGameSlot();
		}

		protected override bool CanPurchase()
		{
			return gameRoom.CanAddGameSlot;
		}
	}

	public abstract class PurchaseAction<T> : Action<T>
	{
		public sealed override string Name { get; protected set; }
		private readonly uint cost;

		protected PurchaseAction(string name, uint cost)
		{
			Name = $"{name}: {cost}$";
			this.cost = cost;
		}

		protected abstract void Purchase(T type);

		public override bool CanExecute(PlayerWallet wallet)
		{
			return wallet.CheckWalletFor(cost);
		}

		public override void Execute(PlayerWallet wallet, T param)
		{
			wallet.Withdraw(cost);
			Purchase(param);
		}
	}

	//public class ActionData
	//{
	//	public PlayerWallet Wallet { get;}

	//	public ActionData(PlayerWallet w)
	//	{
	//		Wallet = w;
	//	}
	//}
}
