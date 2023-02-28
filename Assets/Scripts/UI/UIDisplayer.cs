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
				if (selectionAction is CasinoIdler.Action<GameTypes> typedAction)
				{
					SetUpTypedUIAction(typedAction, selection);
				}
				else if (selectionAction is CasinoIdler.Action walletAction)
				{
					SetUpUIAction(walletAction, selection);
				}
				else
				{
					Debug.Log("Action was neither a typedAction nor walletAction");
				}
			}
		}

		private void SetUpTypedUIAction(CasinoIdler.Action<GameTypes> typedWalletAction, SelectableUI selectableUI)
		{
			GameObject go = Object.Instantiate(frontendUI.TypedUIActionPrefab, frontendUI.SelectableUIActionsContainer.transform);
			TypedUIAction typedUIAction = go.GetComponent<TypedUIAction>();
			typedUIAction.Action = typedWalletAction;
			typedUIAction.SelectableUI = selectableUI;
			activeActions.Add(typedUIAction);

			typedUIAction.TypeIcon.sprite = casinoSprites.GetSpriteByType(gameTypeOptions[typedUIAction.DisplayedOptionIndex]);
			typedUIAction.TypedActionButtonPressed += OnTypedButtonPress;

			typedUIAction.ActionButton.interactable = typedWalletAction.CanExecute(playerWallet);
			typedUIAction.ActionNameText.text = typedWalletAction.Name;
			typedUIAction.ActionButtonPressed += OnTypedActionButtonPressed;
		}

		private void SetUpUIAction(CasinoIdler.Action walletAction, SelectableUI selectableUI)
		{
			GameObject go = Object.Instantiate(frontendUI.UIActionPrefab, frontendUI.SelectableUIActionsContainer.transform);
			UIAction uiAction = go.GetComponent<UIAction>();
			uiAction.Action = walletAction;
			uiAction.SelectableUI = selectableUI;
			activeActions.Add(uiAction);

			uiAction.ActionButton.interactable = walletAction.CanExecute(playerWallet);
			uiAction.ActionNameText.text = walletAction.Name;
			uiAction.ActionButtonPressed += OnActionButtonPress;
		}

		private void OnTypedButtonPress(TypedUIAction typedUIAction)
		{
			typedUIAction.DisplayedOptionIndex++;
			if (typedUIAction.DisplayedOptionIndex > gameTypeOptions.Count - 1)
				typedUIAction.DisplayedOptionIndex = 0;
			typedUIAction.TypeIcon.sprite = casinoSprites.GetSpriteByType(gameTypeOptions[typedUIAction.DisplayedOptionIndex]);
		}

		private void OnActionButtonPress(UIAction uiAction)
		{
			CasinoIdler.Action action = uiAction.Action as CasinoIdler.Action;

			action.Execute(playerWallet);

			uiAction.SelectableUI.OnAction(action);

			ClearAllActions();
		}

		private void OnTypedActionButtonPressed(UIAction uiAction)
		{
			if (uiAction.Action is CasinoIdler.Action<GameTypes> typedAction && uiAction is TypedUIAction typedUIAction)
			{
				typedAction.Execute(playerWallet, gameTypeOptions[typedUIAction.DisplayedOptionIndex]);

				uiAction.SelectableUI.OnAction(typedAction);

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
