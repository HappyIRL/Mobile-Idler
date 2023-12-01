using System;
using System.Collections.Generic;
using CasinoIdler;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.UI
{
	class UIDisplayer
	{
		private Selector selector;
		private FrontendUI frontendUI;
		private PlayerWallet playerWallet;
		private ICollection<IAction> selectionActions = new List<IAction>();
		private CasinoSprites casinoSprites;
		private List<GameTypes> gameTypeOptions = new List<GameTypes>((GameTypes[])Enum.GetValues(typeof(GameTypes)));
		private List<UIAction> activeActions = new List<UIAction>();

		public UIDisplayer(Selector selector, PlayerWallet playerWallet, FrontendUI frontendUI, CasinoSprites casinoSprites)
		{
			this.casinoSprites = casinoSprites;
			this.playerWallet = playerWallet;
			this.frontendUI = frontendUI;
			this.selector = selector;
			selector.NewSelection += OnNewSelection;
		}

		private void OnNewSelection()
		{
			if (selector.Selection == null)
				return;

			if (selector.Selection != selector.OldSelection)
				selectionActions = selector.Selection.Selectable.GetActions();

			if(selectionActions == null)
				Debug.Log("No actions were found!");

			DisplaySelection(selector.Selection);
		}

		private void DisplaySelection(SelectableUI selection)
		{
			foreach (var activeAction in activeActions)
			{
				Object.Destroy(activeAction.gameObject);
			}
			activeActions.Clear();

			frontendUI.SelectableUiActionsHeader.text = selection.Selectable.Name;

			foreach (IAction selectionAction in selectionActions)
			{
				switch (selectionAction)
				{
					case CasinoIdler.Action<GameTypes, int, int> typedPositionalAction:
						SetUpTypedPositionalUIAction(typedPositionalAction, selection);
						break;
					case CasinoIdler.Action<int, int> positionalAction:
						SetUpPositionalUIAction(positionalAction, selection);
						break;
					case CasinoIdler.Action walletAction:
						SetUpWalletUIAction(walletAction, selection);
						break;
					default:
						throw new InvalidOperationException("The selected Action could not be matched!");
				}
			}
		}

		private void SetUpTypedPositionalUIAction(CasinoIdler.Action<GameTypes, int, int> typedPositionalWalletAction, SelectableUI selectableUI)
		{
			GameObject go = Object.Instantiate(frontendUI.TypedUIActionPrefab, frontendUI.SelectableUIActionsContainer.transform);
			TypedUIAction typedUIAction = go.GetComponent<TypedUIAction>();
			typedUIAction.Action = typedPositionalWalletAction;
			typedUIAction.SelectableUI = selectableUI;
			activeActions.Add(typedUIAction);

			typedUIAction.TypeIcon.sprite = casinoSprites.GetSpriteByType(gameTypeOptions[typedUIAction.DisplayedOptionIndex]);
			typedUIAction.TypedActionButtonPressed += OnTypedButtonPress;

			typedUIAction.ActionButton.interactable = typedPositionalWalletAction.CanExecute(playerWallet);
			typedUIAction.ActionNameText.text = typedPositionalWalletAction.Name;
			typedUIAction.ActionButtonPressed += OnTypedPositionalActionButtonPressed;
		}

		private void SetUpPositionalUIAction(CasinoIdler.Action<int, int> positionalAction, SelectableUI selectableUI)
		{
			GameObject go = Object.Instantiate(frontendUI.UIActionPrefab, frontendUI.SelectableUIActionsContainer.transform);
			UIAction uiAction = go.GetComponent<UIAction>();
			uiAction.Action = positionalAction;
			uiAction.SelectableUI = selectableUI;
			activeActions.Add(uiAction);

			uiAction.ActionButton.interactable = positionalAction.CanExecute(playerWallet);
			uiAction.ActionNameText.text = positionalAction.Name;
			uiAction.ActionButtonPressed += OnPositionalActionButtonPress;
		}

		private void SetUpWalletUIAction(CasinoIdler.Action walletAction, SelectableUI selectableUI)
		{
			GameObject go = Object.Instantiate(frontendUI.UIActionPrefab, frontendUI.SelectableUIActionsContainer.transform);
			UIAction uiAction = go.GetComponent<UIAction>();
			uiAction.Action = walletAction;
			uiAction.SelectableUI = selectableUI;
			activeActions.Add(uiAction);

			uiAction.ActionButton.interactable = walletAction.CanExecute(playerWallet);
			uiAction.ActionNameText.text = walletAction.Name;
			uiAction.ActionButtonPressed += OnWalletActionPress;
		}

		private void OnTypedButtonPress(TypedUIAction typedUIAction)
		{
			typedUIAction.DisplayedOptionIndex++;
			if (typedUIAction.DisplayedOptionIndex > gameTypeOptions.Count - 1)
				typedUIAction.DisplayedOptionIndex = 0;
			typedUIAction.TypeIcon.sprite = casinoSprites.GetSpriteByType(gameTypeOptions[typedUIAction.DisplayedOptionIndex]);
		}

		private void OnTypedPositionalActionButtonPressed(UIAction uiAction)
		{
			if (uiAction.Action is CasinoIdler.Action<GameTypes, int, int> typedPositionalAction && uiAction is TypedUIAction typedUIAction)
			{
				typedPositionalAction.Execute(playerWallet, gameTypeOptions[typedUIAction.DisplayedOptionIndex], selector.SelectedPosition.x, selector.SelectedPosition.y);

				uiAction.SelectableUI.OnAction(typedPositionalAction.actionType, selector.SelectedPosition);

				ClearAllActions();
			}
		}

		private void OnPositionalActionButtonPress(UIAction uiAction)
		{
			if (uiAction.Action is CasinoIdler.Action<int, int> positionalAction)
			{
				positionalAction.Execute(playerWallet, selector.SelectedPosition.x, selector.SelectedPosition.y);

				uiAction.SelectableUI.OnAction(positionalAction.actionType, selector.SelectedPosition);

				ClearAllActions();
			}
		}

		private void OnWalletActionPress(UIAction uiAction)
		{
			if (uiAction.Action is CasinoIdler.Action walletAction)
			{
				walletAction.Execute(playerWallet);

				uiAction.SelectableUI.OnAction(walletAction.actionType, selector.SelectedPosition);

				ClearAllActions();
			}
		}

		private void ClearAllActions()
		{
			foreach (var activeAction in activeActions)
			{
				Object.Destroy(activeAction.gameObject);
			}
			activeActions.Clear();
			frontendUI.SelectableUiActionsHeader.text = "";
		}

		public void OnTick()
		{
			frontendUI.PlayerMoneyUiText.text = playerWallet.Wallet.ToString("0$");
			foreach (var activeAction in activeActions)
			{
				activeAction.ActionButton.interactable = activeAction.Action.CanExecute(playerWallet);
			}
		}
	}
}
