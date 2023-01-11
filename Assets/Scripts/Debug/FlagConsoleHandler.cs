using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FlagConsoleHandler : MonoBehaviour
{
	private void Start()
	{
		if(FlagHandler._alwaysLoadNewFlag)
			Debug.LogWarning("_alwaysLoadNewFlag IS ENABLED! THE GAME STARTS WITHOUT LOADING A SAVE FILE");
	}
}
