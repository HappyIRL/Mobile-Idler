using UnityEngine;

[System.Serializable]
public struct SerializedVector2
{
	public float X, Y;

	public SerializedVector2(float x, float y)
	{
		X = x;
		Y = y;
	}

	public static implicit operator Vector2(SerializedVector2 serializedVector2)
	{
		return new Vector2(serializedVector2.X, serializedVector2.Y);
	}

	public static implicit operator SerializedVector2(Vector2 unityVec)
	{
		return new SerializedVector2 { X = unityVec.x, Y = unityVec.y };
	}
}