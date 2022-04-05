using System;
using System.Collections.Generic;
using System.Linq;
using CasinoIdler;
using UnityEditor;
using UnityEngine;
using Action = CasinoIdler.Action;

public class OnGUISceneView : MonoBehaviour
{
	private PrototypeSelector selector;
	private PlayerWallet wallet;
	private ISelectable rootSelectable;

	private ISelectable oldSelection;
	private ICollection<IAction> actions;
	private LinkedList<GameTypes> selectedGameTypeOptions = new LinkedList<GameTypes>((GameTypes[])Enum.GetValues(typeof(GameTypes)));
	private bool isInitComplete;
	private bool selectableToggle = true;

	public void Init( PlayerWallet wallet, PrototypeSelector selector, ISelectable rootSelectable)
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
		EditorGUILayout.BeginHorizontal();
		GUIDrawActions();
		EditorGUILayout.EndHorizontal();
		GUIDrawCash();
	}

	private void GUIDrawSelected(ISelectable selectable)
	{
		if (GUILayout.Button(selectable.Name))
		{
			selector.SetSelectable(selectable);
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
		{
			actions = null;
			return;
		}

		if (selector.Selection != oldSelection)
		{
			actions = selector.Selection.GetActions();
			oldSelection = selector.Selection;
		}

		if (actions == null)
			return;

		foreach (var actionBase in actions)
		{
			EditorGUI.BeginDisabledGroup(!actionBase.CanExecute(wallet));

			if (actionBase is CasinoIdler.Action<GameTypes> typedAction)
			{
				if (GUILayout.Button($"Type of buyable: {selectedGameTypeOptions.First()}"))
				{
					GameTypes t = selectedGameTypeOptions.First();
					selectedGameTypeOptions.RemoveFirst();
					selectedGameTypeOptions.AddLast(t);
				}

				if (GUILayout.Button(typedAction.Name))
				{
					typedAction.Execute(wallet, selectedGameTypeOptions.First());
				}
			}
			else if (actionBase is Action action)
			{

				if (GUILayout.Button(action.Name))
				{
					action.Execute(wallet);
				}
			}

			EditorGUI.EndDisabledGroup();
		}
	}
}
