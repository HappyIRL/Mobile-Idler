using System;
using System.Collections.Generic;
using Assets.Scripts.Utils;
using CasinoIdler;
using Action = System.Action;

public class GameFloor : ISelectable
{
	public string Name => "Floor";
	public Action Unselect { get; set; }
	public Action InternalStructureChanged { get; set; }
	public IReadOnlyTwoDimensionalArray<GameRoom> GameRooms => new ReadOnlyTwoDimensionalArray<GameRoom>(gameRooms);
	public bool CanAddGameRoom => gameRoomCount < maxGameRooms;

	private GameRoom[,] gameRooms = new GameRoom[CasinoUIConstants.FLOOR_COLS / 2, CasinoUIConstants.FLOOR_ROWS / 2];
	private List<IAction> actions = new List<IAction>();
	private const uint BaseGameRoomCost = 5;
	private int gameRoomCount;
	private int maxGameRooms => gameRooms.Length;

	public GameFloor(GameFloorData data, bool isTutorial)
	{
		if (isTutorial)
		{
			CreateGameRoom(GetBaseGameRoomData());
			return;
		}

		if (data.GameRoomsData != null)
		{
			foreach (var roomData in data.GameRoomsData)
			{
				CreateGameRoom(roomData);
			}
		}
	}

	public GameFloorData FetchData()
	{
		GameFloorData data = new GameFloorData();

		int rows = gameRooms.GetLength(0);
		int columns = gameRooms.GetLength(1);

		GameRoomData[,] gameRoomsData = new GameRoomData[rows, columns];

		for (int i = 0; i < rows; i++)
		{
			for (int j = 0; j < columns; j++)
			{
				if(gameRooms[i,j] != null)
					gameRoomsData[i, j] = gameRooms[i, j].FetchData();
			}
		}

		data.GameRoomsData = gameRoomsData;

		return data;
	}

	public void CreateNewGameRoom(GameTypes type, int floorPosX, int floorPosY)
	{
		GameRoomData data = GetBaseGameRoomData();

		data.GameType = type;
		data.IsTutorialRoom = false;
		data.posX = floorPosX / 2;
		data.posY = (CasinoUIConstants.LAST_FLOOR_ROWS_INDEX - floorPosY) / 2;

		CreateGameRoom(data);
	}

	public uint RemoveGameRoom(GameRoom gameRoom)
	{
		int cols = gameRooms.GetLength(0);
		int rows = gameRooms.GetLength(1);

		int removeCol = -1;
		int removeRow = -1;

		for (int i = 0; i < cols; i++)
		{
			for (int j = 0; j < rows; j++)
			{
				if (gameRooms[i, j] == gameRoom)
				{
					removeCol = i;
					removeRow = j;
					break;
				}
			}

			if (removeCol != -1)
				break;
		}

		if (removeCol != -1)
		{
			gameRooms[removeCol, removeRow] = null;
		}
		else
		{
			throw new InvalidOperationException("The specified game room was not found in the array.");
		}

		gameRoom.Unselect?.Invoke();
		gameRoomCount--;

		InternalStructureChanged?.Invoke();

		return BaseGameRoomCost;
	}

	public ICollection<IAction> GetActions()
	{
		return actions;
	}

	public void InitActions(IAction[] toAddAction)
	{
		actions.AddRange(toAddAction);
		actions.Add(new PurchaseGameRoomAction(this, "Buy GameRoom", BaseGameRoomCost));
	}

	public uint GetProductionRate()
	{
		uint value = 0;

		for (int i = 0; i < gameRooms.GetLength(0); i++)
		{
			for (int j = 0; j < gameRooms.GetLength(1); j++)
			{
				if(gameRooms[i,j] != null)
					value += gameRooms[i, j].GetProductionRate(); 
			}
		}

		return value;
	}

	private void CreateGameRoom(GameRoomData data)
	{
		GameRoom gameRoom = new GameRoom(data);

		IAction[] gameRoomActions = { new SellGameRoomAction(this, gameRoom, "Sell GameRoom", 5) };
		gameRoom.InitActions(gameRoomActions);
		gameRoom.InternalStructureChanged += OnInternalStructureChanged;

		gameRooms[data.posX, data.posY] = gameRoom;
		gameRoomCount++;

		InternalStructureChanged?.Invoke();
	}

	private GameRoomData GetBaseGameRoomData()
	{
		GameRoomData data = new GameRoomData
		{
			GameType = GameTypes.Roulette,
			IsTutorialRoom = true
		};

		return data;
	}

	private void OnInternalStructureChanged()
	{
		InternalStructureChanged?.Invoke();
	}
}

[Serializable]
public struct GameFloorData
{
	public GameRoomData[,] GameRoomsData;
}
