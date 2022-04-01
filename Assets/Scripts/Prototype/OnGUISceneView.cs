using System.Collections.Generic;
using CasinoIdler;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Action = CasinoIdler.Action;

public class OnGUISceneView : MonoBehaviour
{
	private PrototypeSelector selector;
	private PlayerWallet wallet;
	private ISelectable rootSelectable;

	private bool isInitComplete;

	public void Init( PlayerWallet wallet ,PrototypeSelector selector, ISelectable rootSelectable)
	{
		this.selector = selector;
		this.wallet = wallet;
		this.rootSelectable = rootSelectable;
		isInitComplete = true;
	}

	private void OnGUI()
	{
		if (!isInitComplete)
			return;

		GUIDrawSelected(rootSelectable);
		GUIDrawActions();
		GUIDrawCash();
	}

	private void GUIDrawSelected(ISelectable selectable)
	{
		if (GUILayout.Button(selectable.Name))
		{
			selector.Selection = selectable;
		}

		if (selectable.SubSelections == null)
			return;

		for (int i = 0; i < selectable.SubSelections.Count; i++)
		{
			GUIDrawSelected(selectable.SubSelections[i]);
		}
	}

	private void GUIDrawCash()
	{
		GUILayout.Label(wallet.Wallet.ToString("0.00$"));
	}

	private void GUIDrawActions()
	{
		if (selector.Selection == null)
			return;

		ICollection<IAction> actions = selector.Selection.GetActions();

		if (actions == null)
			return;

		ActionData data = new ActionData(wallet);


		foreach (var actionBase in actions)
		{
			if (actionBase is Action<GameTypes> typedAction)
			{
				//if(button for enum -> pass value from dropdown)
				//EditorGUILayout.EnumPopup
				//options for GameTypes
				//
			}
			else if (actionBase is Action action)
			{
				EditorGUI.BeginDisabledGroup(!action.CanExecute(data));

				if (GUILayout.Button(action.Name))
				{
					action.Execute(data);
				}

				EditorGUI.EndDisabledGroup();
			}
		}
	}
}
