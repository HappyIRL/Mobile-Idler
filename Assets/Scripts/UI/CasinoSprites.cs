using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/CasinoSprites")]
public class CasinoSprites : ScriptableObject
{
	[SerializeField] private Sprite rouletteSprite;
	[SerializeField] private Sprite blackJackSprite;
	[SerializeField] private Sprite slotSprite;

	[SerializeField] private Sprite rouletteRoomSprite;
	[SerializeField] private Sprite blackJackRoomSprite;
	[SerializeField] private Sprite slotRoomSprite;
	[SerializeField] private Sprite floorSprite;

	public Sprite GetSpriteByType(GameTypes type)
	{
		switch (type)
		{
			case GameTypes.Roulette:
				return rouletteSprite;
			case GameTypes.Blackjack:
				return blackJackSprite;
			case GameTypes.SlotMachine:
				return slotSprite;
			default:
				return rouletteSprite;
		}
	}

	public Sprite GetRoomSpriteByType(GameTypes type)
	{
		switch (type)
		{
			case GameTypes.Roulette:
				return rouletteRoomSprite;
			case GameTypes.Blackjack:
				return blackJackRoomSprite;
			case GameTypes.SlotMachine:
				return slotRoomSprite;
			default:
				return floorSprite;
		}
	}

	public Sprite GetFloorSprite()
	{
		return floorSprite;
	}
}
