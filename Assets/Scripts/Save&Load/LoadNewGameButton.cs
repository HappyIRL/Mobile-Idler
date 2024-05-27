using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadNewGameButton : MonoBehaviour
{
	[Zenject.Inject] private SaveHandler saveHandler;

	public void OnButtonClick()
	{
		saveHandler.LoadNew();
	}
}
