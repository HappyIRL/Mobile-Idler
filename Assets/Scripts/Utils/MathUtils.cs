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
