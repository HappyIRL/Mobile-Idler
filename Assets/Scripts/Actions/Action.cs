using System.Numerics;
using NaughtyAttributes.Test;

namespace CasinoIdler
{
	public interface IAction
	{
		public abstract string Name { get; }
		public abstract ActionType actionType { get; }
		public abstract bool CanExecute(PlayerWallet wallet);
	}

	public abstract class Action : IAction
	{
		public virtual string Name { get; protected set; }
		public abstract ActionType actionType { get; }
		public abstract bool CanExecute(PlayerWallet wallet);
		public abstract void Execute(PlayerWallet wallet);
	}

	public abstract class Action<T1, T2, T3> : IAction
	{
		public virtual string Name { get; protected set; }
		public abstract ActionType actionType { get; }
		public abstract bool CanExecute(PlayerWallet wallet);
		public abstract void Execute(PlayerWallet wallet, T1 param1, T2 pram2, T3 param3);
	}

	public abstract class Action<T1, T2> : IAction
	{
		public virtual string Name { get; protected set; }
		public abstract ActionType actionType { get; }
		public abstract bool CanExecute(PlayerWallet wallet);
		public abstract void Execute(PlayerWallet wallet, T1 param1, T2 param2);
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

	public abstract class PurchaseAction<T1, T2> : Action<T1, T2>
	{
		public sealed override string Name { get; protected set; }
		private readonly uint cost;

		protected PurchaseAction(string name, uint cost)
		{
			this.cost = cost;
			Name = $"{name}: {cost}$";
		}

		protected abstract void Purchase(T1 posX, T2 posY);

		protected abstract bool CanPurchase();

		public override bool CanExecute(PlayerWallet wallet)
		{
			return CanPurchase() && wallet.CheckWalletFor(cost);
		}

		public override void Execute(PlayerWallet wallet, T1 posX, T2 posY)
		{
			wallet.Withdraw(cost);
			Purchase(posX, posY);
		}
	}

	public class PurchaseGameFloorAction : PurchaseAction
	{
		public override ActionType actionType => ActionType.Purchase;

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

	public class PurchaseGameSlotAction : PurchaseAction<int, int>
	{
		public override ActionType actionType => ActionType.Purchase;

		private readonly GameRoom gameRoom;

		public PurchaseGameSlotAction(GameRoom gameRoom, string name, uint cost) : base(name, cost)
		{
			this.gameRoom = gameRoom;
		}

		protected override void Purchase(int posX, int posY)
		{
			gameRoom.CreateNewGameSlot(posX, posY);
		}

		protected override bool CanPurchase()
		{
			return gameRoom.CanAddGameSlot;
		}
	}

	public abstract class SellAction : Action
	{
		public sealed override string Name { get; protected set; }

		protected uint value;

		protected SellAction(string name, uint defaultValue)
		{
			Name = $"{name}: {defaultValue}$";
			value = defaultValue;
		}

		protected abstract uint Sell();

		public override bool CanExecute(PlayerWallet wallet)
		{
			return true;
		}

		public override void Execute(PlayerWallet wallet)
		{
			wallet.Deposit(Sell());
		}
	}

	public class SellGameFloorAction : SellAction
	{
		public override ActionType actionType => ActionType.Sell;

		private readonly Casino casino;
		private readonly GameFloor gameFloor;

		public SellGameFloorAction(Casino casino, GameFloor gameFloor, string name, uint defaultValue) : base(name, defaultValue)
		{
			this.casino = casino;
			this.gameFloor = gameFloor;
		}

		protected override uint Sell()
		{
			return casino.RemoveGameFloor(gameFloor);
		}
	}

	public class SellGameRoomAction : SellAction
	{
		public override ActionType actionType => ActionType.Sell;

		private readonly GameFloor gameFloor;
		private readonly GameRoom gameRoom;

		public SellGameRoomAction(GameFloor gameFloor, GameRoom gameRoom, string name, uint defaultValue) : base(name, defaultValue)
		{
			this.gameFloor = gameFloor;
			this.gameRoom = gameRoom;
		}

		protected override uint Sell()
		{
			return gameFloor.RemoveGameRoom(gameRoom);
		}
	}

	public class SellGameSlotAction : SellAction
	{
		public override ActionType actionType => ActionType.Sell;

		private readonly GameRoom gameRoom;
		private readonly GameSlot gameSlot;

		public SellGameSlotAction(GameRoom gameRoom, GameSlot gameSlot, string name, uint defaultValue) : base(name, defaultValue)
		{
			this.gameRoom = gameRoom;
			this.gameSlot = gameSlot;
		}

		protected override uint Sell()
		{
			return gameRoom.RemoveGameSlot(gameSlot);
		}
	}

	public abstract class PurchaseAction<T1, T2, T3> : Action<T1, T2, T3>
	{

		public sealed override string Name { get; protected set; }
		private readonly uint cost;

		protected PurchaseAction(string name, uint cost)
		{
			Name = $"{name}: {cost}$";
			this.cost = cost;
		}

		protected abstract void Purchase(T1 type, T2 posX, T3 posY);
		protected abstract bool CanPurchase();

		public override bool CanExecute(PlayerWallet wallet)
		{
			return wallet.CheckWalletFor(cost) && CanPurchase();
		}

		public override void Execute(PlayerWallet wallet, T1 type, T2 posX, T3 posY)
		{
			wallet.Withdraw(cost);
			Purchase(type, posX, posY);
		}
	}

	public class PurchaseGameRoomAction : PurchaseAction<GameTypes, int, int>
	{ 
		public override ActionType actionType => ActionType.Purchase;

		private readonly GameFloor gameFloor;

		public PurchaseGameRoomAction(GameFloor gameFloor, string name, uint cost) : base(name, cost)
		{
			this.gameFloor = gameFloor;
		}

		protected override void Purchase(GameTypes type, int posX, int posY)
		{
			gameFloor.CreateNewGameRoom(type, posX, posY);
		}

		protected override bool CanPurchase()
		{
			return gameFloor.CanAddGameRoom;
		}
	}

	public abstract class UpgradeAction : Action
	{
		public sealed override string Name { get; protected set; }

		protected readonly string originalName;

		protected UpgradeAction(string name, uint defaultUpgradeCost)
		{
			originalName = name;
			Name = $"{originalName}: {defaultUpgradeCost}$";
		}

		protected abstract void Upgrade();

		protected abstract bool CanUpgrade();

		protected abstract uint GetCost();

		public override bool CanExecute(PlayerWallet wallet)
		{
			return CanUpgrade() && wallet.CheckWalletFor(GetCost());
		}

		public override void Execute(PlayerWallet wallet)
		{
			wallet.Withdraw(GetCost());
			Upgrade();
		}
	}

	public class GameSlotUpgradeAction : UpgradeAction
	{
		public override ActionType actionType => ActionType.Upgrade;

		private readonly GameSlot gameSlot;

		public GameSlotUpgradeAction(GameSlot gameSlot, string name, uint defaultUpgradeCost) : base(name, defaultUpgradeCost)
		{
			this.gameSlot = gameSlot;
		}

		protected override void Upgrade()
		{
			gameSlot.Upgrade();

			if (!CanUpgrade())
			{
				Name = $"{originalName}: MAXED";
				return;
			}

			Name = $"{originalName}: {GetCost()}$";
		}

		protected override bool CanUpgrade()
		{
			return gameSlot.CanUpgrade();
		}

		protected override uint GetCost()
		{
			return gameSlot.GetUpgradeCost();
		}
	}

	public enum ActionType
	{
		Sell,
		Purchase,
		Upgrade
	}

	public enum ActionedType
	{
		Casino,
		GameFloor,
		GameRoom,
		GameSlot
	}
}
