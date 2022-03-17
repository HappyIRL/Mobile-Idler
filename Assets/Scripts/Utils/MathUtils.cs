using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathUtils
{
	/// <summary>
	/// Int to uint dash calculation
	/// </summary>
	/// <param name="modifier"></param>
	/// <param name="toModify"></param>
	public static void ModifyUint(int modifier, ref uint toModify)
    {
	    if (modifier > 0)
		    toModify += (uint)modifier;
	    else
		    toModify -= (uint)modifier;
    }

	/// <summary>
	/// Int to ulong dash calculation
	/// </summary>
	/// <param name="modifier"></param>
	/// <param name="toModify"></param>
	public static void ModifyUlong(int modifier, ref ulong toModify)
	{
		if (modifier > 0)
			toModify += (ulong)modifier;
		else
			toModify -= (ulong)modifier;
	}
}

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
