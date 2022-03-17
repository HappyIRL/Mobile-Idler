using System;
using UnityEditor;
using UnityEngine;

public class OnGUISceneView : MonoBehaviour
{
	private ActionExecutor actionExecutor;

	private bool isInitComplete;

	public void Init(ActionExecutor actionExecutor)
	{
		this.actionExecutor = actionExecutor;
		isInitComplete = true;
	}

	private void OnGUI()
	{
		if (!isInitComplete)
			return;

		GUIDrawSelected();
		GUIDrawActions();
		GUIDrawCash();
	}

	private void GUIDrawSelected()
	{
		//selected is gameslot its type is roulette etc
	}

	private void GUIDrawCash()
	{
		GUILayout.Label(actionExecutor.GetWalletAmount().ToString("0.00$"));
	}

	private void GUIDrawActions()
	{
		CasinoIdler.ActionDisplayData[] datas = actionExecutor.GetActionsDisplayData();

		for (var i = 0; i < datas.Length; i++)
		{
			var data = datas[i];

			EditorGUI.BeginDisabledGroup(!data.isDisplayable);

			if (GUILayout.Button(data.name))
				actionExecutor.ExecuteAction(i);

			EditorGUI.EndDisabledGroup();
		}
	}

	private void OnDisable()
	{
		isInitComplete = false;
	}
}
