using System;
using System.Collections.Generic;
using CasinoIdler;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PrototypeUI : PlayerInputEventsBehaviour
{
	private Selector selector;
	private PlayerWallet wallet;
	private ISelectable rootSelectable;
	private ICollection<IAction> actions;
	private Sprite test;
	private LinkedList<GameTypes> selectedGameTypeOptions = new LinkedList<GameTypes>((GameTypes[])Enum.GetValues(typeof(GameTypes)));
	private bool isInitComplete;
	private Vector2 scrollPosition;
	private ISelectable[,] floorGrid = new ISelectable[8,18];

	private List<ISelectable> floors = new List<ISelectable>();

	private Tilemap floorMap;

	public void Init(PlayerWallet wallet, Selector selector, ISelectable rootSelectable)
	{
		this.selector = selector;
		this.wallet = wallet;
		this.rootSelectable = rootSelectable;
		isInitComplete = true;

		floorMap = GameObject.Find("Floor").GetComponent<Tilemap>();

		PopulateCasino();
	}

	/// <summary>
	/// How can I select an ISelectable?
	/// The UI knows by default the size and limit of the casino.
	/// The UI creates its own idea of how the casino shall look (size per room, position of rooms, position and size of slots etc).
	/// It translates cursor click data into its own casino matrix it created and is then able to set the current selection.
	/// It searches if the click was on the floor rect, if so, it then searches if it was on a gameroom rect, if not it just selects the floor, if so it goes further and checks if it was on a gameslot.
	/// </summary>

	[NaughtyAttributes.Button("pressMEPLSSS")]
	private void PopulateCasino()
	{
		foreach (var floor in rootSelectable.SubSelections)
		{
			floors.Add(floor);
			SelectFloor(0);
			foreach (var room in floor.SubSelections)
			{
				SetUpRoomUI(room);
				foreach (var gameSlot in room.SubSelections)
				{
					SetUpGameSlotUI(gameSlot);
				}
			}
		}
	}

	private void SelectFloor(int index)
	{
		ISelectable floor = floors[index];
		DrawFloor(floor);
	}

	private void DrawFloor(ISelectable floor)
	{
		Rect floorRect = new Rect(new Vector2(4, 12), new Vector2(8, 18));

		foreach (var room in floor.SubSelections)
		{
			DrawRoom(room, new Vector3Int(0,0,0));
		}
	}

	private void DrawRoom(ISelectable room, Vector3Int position)
	{
		floorMap.ClearAllTiles();
		for (int i = 0; i < 2; i++)
		{
			for (int j = 0; j < 2; j++)
			{
				int index = i * 2 + j;
				if (index < room.SubSelections.Count)
				{
					ISelectable selectable = room.SubSelections[index];
					Vector3Int pos = new Vector3Int(position.x + j, position.y - i, 0);
					Tile tile = ScriptableObject.CreateInstance<Tile>();
					test = Resources.Load<Sprite>("purple_rug");
					if(test == null)
						Debug.Log("null");
					tile.sprite = test;
					floorMap.SetTile(pos, tile);
				}
			}
		}
	}

	private void SetUpRoomUI(ISelectable room)
	{

	}

	private void SetUpGameSlotUI(ISelectable gameslot)
	{

	}

	

	private void GetSelectionAtPos(Vector2 worldSpacePos)
	{
		
	}

	private void GUIDrawSelection(ISelectable selectable)
	{
		if (GUILayout.Button(selectable.Name))
		{
			selector.SetSelection(selectable);
		}

		if (selectable.SubSelections == null)
			return;

		for (int i = 0; i < selectable.SubSelections.Count; i++)
		{
			GUIDrawSelection(selectable.SubSelections[i]);
		}
	}
}

[Serializable]
public struct UIData
{

}
