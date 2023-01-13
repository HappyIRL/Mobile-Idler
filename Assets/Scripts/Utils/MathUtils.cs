using UnityEngine;

[System.Serializable]
public struct SerializedVector3
{
	public float X, Y, Z;

	public SerializedVector3(Vector3 v3)
	{
		X = v3.x;
		Y = v3.y;
		Z = v3.z;
	}

	public Vector3 ToVector3()
	{
		return new Vector3(X, Y, Z);
	}
}
