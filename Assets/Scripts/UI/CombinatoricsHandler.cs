using Assets.Scripts.UI;
using Assets.Scripts.Utils;
using System.Collections.Generic;
using UnityEngine;

public class CombinatoricsHandler
{
	public IReadOnlyList<GameFloor> gameFloors;

	public float TotalScore => totalScore;

	private float totalScore = 1;
	private Casino casino;

	public CombinatoricsHandler(Casino casino)
	{
		this.casino = casino;
		gameFloors = casino.GameFloors;
		casino.InternalStructureChanged += OnCasinoStructureChanged;
		OnCasinoStructureChanged();
	}

	private void OnCasinoStructureChanged()
	{
		//reset to 1x
		totalScore = 1;

		float totalBlackJackScore = GetBlackjackScores();

		totalScore += totalBlackJackScore;
	}

	//Returns a multiplier based on the 2nd floors amount of connected BlackJackTables
	private float GetBlackjackScores()
	{
		// Checking if the 2nd floor is unlocked, if not return no multiplier;
		if (gameFloors.Count < 2)
			return 0;

		float totalBlackJackScore = 0;

		GameFloor gameFloor = gameFloors[1];
		
		for (int i = 0; i < gameFloor.GameRooms.Columns; i++)
		{
			for (int j = 0; j < gameFloor.GameRooms.Rows; j++)
			{
				if (gameFloor.GameRooms[i, j]?.GameType == GameTypes.Blackjack)
					totalBlackJackScore += GetBlackjackScore(gameFloor.GameRooms, i, j);
			}
		}

		// Dividing by two to get unique interactions.In this case each interaction can only happen in pairs of two horizontal and vertical, thus they are counted twice on both ends.
		return totalBlackJackScore / 2;
	}

	// Gets the BlackJackScore based on GameTypes next to the provided grid coordinates
	private float GetBlackjackScore(IReadOnlyTwoDimensionalArray<GameRoom> grid, int col, int row)
	{
		float blackJackScore = 0;
		int blackJackConnections = 0;
		int otherConnections = 0;

		// Define the directions: up, down, left, right
		int[] directionsCol = { 0, 0, -1, 1 };
		int[] directionsRow = { -1, 1, 0, 0 };

		for (int i = 0; i < 4; i++)
		{
			int surroundingCol = col + directionsCol[i];
			int surroundingRow = row + directionsRow[i];

			// Check if the new position is within bounds
			if (surroundingRow >= 0 && surroundingRow < grid.Rows && surroundingCol >= 0 && surroundingCol < grid.Columns)
			{
				// Check if it's a Blackjack connection
				if (grid[surroundingCol, surroundingRow]?.GameType == GameTypes.Blackjack)
				{
					blackJackScore += 0.25f;
					blackJackConnections++;
				}
				// Otherwise, count other connections
				else if (grid[surroundingCol, surroundingRow] != null)
				{
					otherConnections++;
				}
			}
		}

		// If there are other connections, return the Blackjack score, otherwise return 0
		return (otherConnections != 0) ? blackJackScore : 0;
	}

	public void UnsubscribeEvents()
	{
		casino.InternalStructureChanged -= OnCasinoStructureChanged;
	}
}
