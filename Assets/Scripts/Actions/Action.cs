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

	public class PurchaseGameFloorAction : PurchaseAction
	{
		private readonly Casino casino;

		public PurchaseGameFloorAction(Casino casino, string name, uint cost) : base(name, cost)
		{
			this.casino = casino;
		}

		protected override void Purchase()
		{
			casino.CreateNewGameFloor();
		}

		protected override bool CanPurchase()
		{
			return true;
		}
	}

	public class PurchaseGameSlotAction : PurchaseAction
	{
		private readonly GameRoom gameRoom;

		public PurchaseGameSlotAction(GameRoom gameRoom, string name, uint cost) : base(name, cost)
		{
			this.gameRoom = gameRoom;
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

	public class SellGameFloorAction : SellAction
	{
		private readonly Casino casino;
		private readonly GameFloor gameFloor;

		public SellGameFloorAction(Casino casino, GameFloor gameFloor, string name) : base(name)
		{
			this.casino = casino;
			this.gameFloor = gameFloor;
		}

		protected override uint Sell()
		{
			return casino.RemoveGameFloor(gameFloor);
		}

		protected override bool CanSell()
		{
			return casino.CanRemoveGameRoom;
		}
	}

	public class SellGameRoomAction : SellAction
	{
		private readonly GameFloor gameFloor;
		private readonly GameRoom gameRoom;

		public SellGameRoomAction(GameFloor gameFloor, GameRoom gameRoom, string name) : base(name)
		{
			this.gameFloor = gameFloor;
			this.gameRoom = gameRoom;
		}

		protected override uint Sell()
		{
			return gameFloor.RemoveGameRoom(gameRoom);
		}

		protected override bool CanSell()
		{
			return gameFloor.CanRemoveGameRoom;
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
		protected abstract bool CanPurchase();

		public override bool CanExecute(PlayerWallet wallet)
		{
			return wallet.CheckWalletFor(cost) && CanPurchase();
		}

		public override void Execute(PlayerWallet wallet, T param)
		{
			wallet.Withdraw(cost);
			Purchase(param);
		}
	}

	public class PurchaseGameRoomAction : PurchaseAction<GameTypes>
	{
		private readonly GameFloor gameFloor;

		public PurchaseGameRoomAction(GameFloor gameFloor, string name, uint cost) : base(name, cost)
		{
			this.gameFloor = gameFloor;
		}

		protected override void Purchase(GameTypes type)
		{
			gameFloor.CreateNewGameRoom(type);
		}

		protected override bool CanPurchase()
		{
			return true;
		}
	}
}
