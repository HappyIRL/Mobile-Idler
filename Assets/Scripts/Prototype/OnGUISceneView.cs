using System;
using System.Collections.Generic;
using System.Linq;
using CasinoIdler;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class OnGUISceneView : MonoBehaviour
{
	//private Selector selector;
	//private PlayerWallet wallet;
	//private ISelectable rootSelectable;
	//private ICollection<IAction> actions;
	//private LinkedList<GameTypes> selectedGameTypeOptions = new LinkedList<GameTypes>((GameTypes[])Enum.GetValues(typeof(GameTypes)));
	//private bool isInitComplete;
	//private Vector2 scrollPosition;

	//public void Init(PlayerWallet wallet, Selector selector, ISelectable rootSelectable)
	//{
	//	this.selector = selector;
	//	this.wallet = wallet;
	//	this.rootSelectable = rootSelectable;
	//	isInitComplete = true;
	//}

	//private void OnGUI()
	//{
	//	if (!isInitComplete)
	//		return;

	//	GUILayout.BeginHorizontal();
	//	GUILayout.BeginVertical();
	//	scrollPosition = GUILayout.BeginScrollView(scrollPosition);
	//	GUIDrawSelection(rootSelectable);
	//	GUILayout.EndVertical();
	//	GUILayout.EndScrollView();

	//	GUILayout.BeginVertical();
	//	GUIDrawActions();
	//	GUILayout.EndVertical();

	//	GUILayout.BeginVertical();
	//	GUIDrawCash();
	//	GUILayout.EndVertical();

	//	GUILayout.EndHorizontal();
	//}

	//private void GUIDrawSelection(ISelectable selectable)
	//{
	//	if (GUILayout.Button(selectable.Name))
	//	{
	//		//selector.SetSelection(selectable);
	//	}

	//	if (selectable.SubSelections == null)
	//		return;

	//	for (int i = 0; i < selectable.SubSelections.Count; i++)
	//	{
	//		GUIDrawSelection(selectable.SubSelections[i]);
	//	}
	//}

	//private void GUIDrawCash()
	//{
	//	GUILayout.Label(wallet.Wallet.ToString("0.00$"));
	//}

	//private void GUIDrawActions()
	//{
	//	if (selector.Selection == null)
	//	{
	//		actions = null;
	//		return;
	//	}

	//	if (selector.Selection != selector.OldSelection)
	//	{
	//		actions = selector.Selection.Selectable.GetActions();
	//	}

	//	if (actions == null)
	//		return;

	//	foreach (var action in actions)
	//	{
	//		GUI.enabled = action.CanExecute(wallet);

	//		if (action is CasinoIdler.Action<GameTypes> typedAction)
	//		{
	//			if (GUILayout.Button($"Type of buyable: {selectedGameTypeOptions.First()}"))
	//			{
	//				GameTypes t = selectedGameTypeOptions.First();
	//				selectedGameTypeOptions.RemoveFirst();
	//				selectedGameTypeOptions.AddLast(t);
	//			}

	//			if (GUILayout.Button(typedAction.Name))
	//			{
	//				typedAction.Execute(wallet, selectedGameTypeOptions.First());
	//			}
	//		}

	//		else if (action is CasinoIdler.Action walletAction)
	//		{

	//			if (GUILayout.Button(walletAction.Name))
	//			{
	//				walletAction.Execute(wallet);
	//			}
	//		}
	//	}
	//}
}
