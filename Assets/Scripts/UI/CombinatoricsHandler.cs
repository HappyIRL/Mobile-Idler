using Assets.Scripts.Utils;
using System.Collections.Generic;
using UnityEngine;

public class CombinatoricsHandler
{
	public IReadOnlyList<GameFloor> gameFloors;

	public uint TotalScore => totalScore;
	private uint totalScore = 1;

	public CombinatoricsHandler(Casino casino)
	{
		gameFloors = casino.GameFloors;
		casino.InternalStructureChanged += OnCasinoStructureChanged;
		OnCasinoStructureChanged();
	}

	private void OnCasinoStructureChanged()
	{
		//reset to 1x
		totalScore = 1;
		uint totalBlackJackScore = 0;

		foreach (GameFloor gameFloor in gameFloors)
		{
			for (int i = 0; i < gameFloor.GameRooms.Columns; i++)
			{
				for (int j = 0; j < gameFloor.GameRooms.Rows; j++)
				{
					if(gameFloor.GameRooms [i,j]?.GameType == GameTypes.Blackjack)
						totalBlackJackScore += GetBlackjackScore(gameFloor.GameRooms, i, j);
				}
			}
		}

		// Dividing by two to get unique interactions.In this case each interaction can only happen in pairs of two horizontal and vertical, thus they are counted twice on both ends.
		totalBlackJackScore /= 2;

		totalScore += totalBlackJackScore;
	}

	//gets the amount of blackjack rooms next to blackjack rooms
	private uint GetBlackjackScore(IReadOnlyTwoDimensionalArray<GameRoom> grid, int col, int row)
	{
		uint blackJackScore = 0;


		// Check up col major
		if (row > 0 && grid[col, row - 1]?.GameType == GameTypes.Blackjack)
			blackJackScore += 1;

		// Check down col major
		if (row < grid.Rows - 1 && grid[col, row + 1]?.GameType == GameTypes.Blackjack)
			blackJackScore += 1;

		// Check left col major
		if (col > 0 && grid[col - 1, row]?.GameType == GameTypes.Blackjack)
			blackJackScore += 1;

		// Check right col major
		if (col < grid.Columns - 1 && grid[col + 1, row]?.GameType == GameTypes.Blackjack)
			blackJackScore += 1;


		return blackJackScore;
	}
}
