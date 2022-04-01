using System;

namespace CasinoIdler
{
	public interface IAction
	{
		public abstract string Name { get;}
	}

	public abstract class Action : IAction
	{
		public virtual string Name { get; protected set; }
		public abstract bool CanExecute(ActionData data);
		public abstract void Execute(ActionData data);
	}

	public abstract class Action<T> : IAction
	{
		public virtual string Name { get; protected set; }
		public abstract bool CanExecute(ActionData data, T param);
		public abstract void Execute(ActionData data, T param);
	}

	public class PurchaseGameRoomAction : PurchaseAction<GameTypes>
	{
		private readonly Casino casino;

		public PurchaseGameRoomAction(string name, uint cost, Casino casino) : base(name, cost)
		{
			this.casino = casino;
		}

		protected override void Purchase()
		{
			casino.CreateNewGameRoom();
		}
	}

	public interface IGameRoomUser
	{
		public void SetGameRoom(GameRoom room);
	}

	public class SellGameRoomAction : Action, IGameRoomUser
	{
		private Casino casino;
		private GameRoom room;

		public SellGameRoomAction(Casino casino, string name)
		{
			this.casino = casino;
			Name = name;
		}

		public override bool CanExecute(ActionData data)
		{
			return true;
		}

		public void SetGameRoom(GameRoom room)
		{
			this.room = room;
		}

		public override void Execute(ActionData data)
		{
			data.Wallet.Deposit(casino.RemoveGameRoom(room));
		}
	}

	public abstract class PurchaseAction : Action
	{
		public sealed override string Name { get; protected set; }
		private readonly uint cost;

		protected PurchaseAction(string name, uint cost)
		{
			Name = $"{cost}$ {name}";
			this.cost = cost;
		}

		protected abstract void Purchase();

		public override bool CanExecute(ActionData data)
		{
			return data.Wallet.CheckWalletFor(cost);
		}

		public override void Execute(ActionData data)
		{
			data.Wallet.Withdraw(cost);
			Purchase();
		}
	}

	public class PurchaseGameSlotAction : PurchaseAction, IGameRoomUser
	{
		private GameRoom gameRoom;

		public PurchaseGameSlotAction(string name, uint cost) : base(name, cost)
		{
		}

		protected override void Purchase()
		{
			gameRoom.CreateNewGameSlot();
		}

		public void SetGameRoom(GameRoom room)
		{
			this.gameRoom = room;
		}
	}

	public abstract class PurchaseAction<T> : Action<T>
	{
		public sealed override string Name { get; protected set; }
		private readonly uint cost;

		protected PurchaseAction(string name, uint cost)
		{
			Name = $"{cost}$ {name}";
			this.cost = cost;
		}

		protected abstract void Purchase();

		public override bool CanExecute(ActionData data, T param)
		{
			return data.Wallet.CheckWalletFor(cost);
		}

		public override void Execute(ActionData data, T param)
		{
			data.Wallet.Withdraw(cost);
			Purchase();
		}
	}

	public class ActionData
	{
		public PlayerWallet Wallet { get;}

		public ActionData(PlayerWallet w)
		{
			Wallet = w;
		}
	}

	public struct ActionDisplayData
	{
		public string Name;
		public bool IsDisplayable;
	}
}
