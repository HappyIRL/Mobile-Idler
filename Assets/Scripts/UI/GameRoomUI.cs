using UnityEngine;
using UnityEngine.Tilemaps;

public class GameRoomUI
{
	private GameRoom gameRoom;
	private Vector3Int floorAnchor;
	private Tilemap tileMap;

	public void Init(GameRoom gameRoom, Tilemap tileMap, Vector3Int floorAnchor)
	{
		this.floorAnchor = floorAnchor;
		this.tileMap = tileMap;
		this.gameRoom = gameRoom;

		DrawGameSlots();
	}

	private void DrawGameSlots()
	{
		for (int i = 0; i < gameRoom.GameSlots.Count; i++)
		{
			GameSlotUI gameSlot = new GameSlotUI();

			int x = i % CasinoUIConstants.GAMEROOM_SIZE + floorAnchor.x;
			int y = -i / CasinoUIConstants.GAMEROOM_SIZE + floorAnchor.y;

			gameSlot.Init(gameRoom.GameSlots[i], tileMap, new Vector3Int(x, y, 0));
		}
	}
}
