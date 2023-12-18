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

			DisplaySelection(selector.Selection);
		}

		private void DisplaySelection(SelectableUI selection)
		{
			ClearActiveActions();

			frontendUI.SelectableUiActionsHeader.text = selection.Selectable.Name;

			if (selection is CasinoUI casinoUI)
			{
				SetUpFloorSelectUIAction(casinoUI);
			}

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

		private void ClearActiveActions()
		{
			foreach (var activeLinkedAction in activeActions)
			{
				Object.Destroy(activeLinkedAction.gameObject);
			}

			activeActions.Clear();

			foreach (var activeAction in activeActions)
			{
				Object.Destroy(activeAction.gameObject);
			}

			activeActions.Clear();
		}

		private void SetUpFloorSelectUIAction(CasinoUI casinoUI)
		{
			GameObject go = Object.Instantiate(frontendUI.FloorSelectUIActionPrefab, frontendUI.SelectableUIActionsContainer.transform);
			UIAction uiAction = go.GetComponent<FloorSelectUIAction>();
			uiAction.SelectableUI = casinoUI;
			activeActions.Add(uiAction);

			uiAction.ActionButton.interactable = true;
			uiAction.ActionNameText.text = casinoUI.CurrentGameFloor.ToString();
			uiAction.ActionButtonPressed += OnFloorSelectButtonPress;
		}


		private void SetUpTypedPositionalUIAction(CasinoIdler.Action<GameTypes, int, int> typedPositionalWalletAction, SelectableUI selectableUI)
		{
			GameObject go = Object.Instantiate(frontendUI.TypedUIActionPrefab, frontendUI.SelectableUIActionsContainer.transform);
			TypedLinkedUIAction typedLinkedUiAction = go.GetComponent<TypedLinkedUIAction>();
			typedLinkedUiAction.Action = typedPositionalWalletAction;
			typedLinkedUiAction.SelectableUI = selectableUI;
			activeActions.Add(typedLinkedUiAction);

			typedLinkedUiAction.TypeIcon.sprite = casinoSprites.GetSpriteByType(gameTypeOptions[typedLinkedUiAction.DisplayedOptionIndex]);
			typedLinkedUiAction.TypedActionButtonPressed += OnTypedButtonPress;

			typedLinkedUiAction.ActionButton.interactable = typedPositionalWalletAction.CanExecute(playerWallet);
			typedLinkedUiAction.ActionNameText.text = typedPositionalWalletAction.Name;
			typedLinkedUiAction.ActionButtonPressed += OnTypedPositionalActionButtonPressed;
		}

		private void SetUpPositionalUIAction(CasinoIdler.Action<int, int> positionalAction, SelectableUI selectableUI)
		{
			GameObject go = Object.Instantiate(frontendUI.UIActionPrefab, frontendUI.SelectableUIActionsContainer.transform);
			LinkedUIAction uiAction = go.GetComponent<LinkedUIAction>();
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
			LinkedUIAction uiAction = go.GetComponent<LinkedUIAction>();
			uiAction.Action = walletAction;
			uiAction.SelectableUI = selectableUI;
			activeActions.Add(uiAction);

			uiAction.ActionButton.interactable = walletAction.CanExecute(playerWallet);
			uiAction.ActionNameText.text = walletAction.Name;
			uiAction.ActionButtonPressed += OnWalletActionPress;
		}

		private void OnTypedButtonPress(TypedLinkedUIAction typedLinkedUiAction)
		{
			typedLinkedUiAction.DisplayedOptionIndex++;
			if (typedLinkedUiAction.DisplayedOptionIndex > gameTypeOptions.Count - 1)
				typedLinkedUiAction.DisplayedOptionIndex = 0;

			typedLinkedUiAction.TypeIcon.sprite = casinoSprites.GetSpriteByType(gameTypeOptions[typedLinkedUiAction.DisplayedOptionIndex]);
		}

		private void OnFloorSelectButtonPress(UIAction uiAction)
		{
			if (uiAction is FloorSelectUIAction floorSelectUIAction && floorSelectUIAction.SelectableUI is IndexedSelectableUI indexedSelectable)
			{
				if (floorSelectUIAction.SelectableUI is CasinoUI casinoUI)
				{
					floorSelectUIAction.DisplayedOptionIndex = casinoUI.CurrentGameFloor;
					floorSelectUIAction.DisplayedOptionIndex++;

					if (floorSelectUIAction.DisplayedOptionIndex >= casinoUI.GameFloors.Count)
						floorSelectUIAction.DisplayedOptionIndex = 0;
				}
					

				floorSelectUIAction.ActionNameText.text = floorSelectUIAction.DisplayedOptionIndex.ToString();
				indexedSelectable.OnIndexedAction(floorSelectUIAction.DisplayedOptionIndex);
			}
		}

		private void OnTypedPositionalActionButtonPressed(UIAction uiAction)
		{
			if (uiAction is TypedLinkedUIAction typedUIAction && typedUIAction.Action is CasinoIdler.Action<GameTypes, int, int> typedPositionalAction)
			{
				typedPositionalAction.Execute(playerWallet, gameTypeOptions[typedUIAction.DisplayedOptionIndex], selector.SelectedPosition.x, selector.SelectedPosition.y);

				typedUIAction.SelectableUI.OnAction(typedPositionalAction.actionType, selector.SelectedPosition);

				ClearAllActions();
			}
		}

		private void OnPositionalActionButtonPress(UIAction uiAction)
		{
			if (uiAction is LinkedUIAction linkedUIAction && linkedUIAction.Action is CasinoIdler.Action<int, int> positionalAction)
			{
				positionalAction.Execute(playerWallet, selector.SelectedPosition.x, selector.SelectedPosition.y);

				linkedUIAction.SelectableUI.OnAction(positionalAction.actionType, selector.SelectedPosition);

				ClearAllActions();
			}
		}

		private void OnWalletActionPress(UIAction uiAction)
		{
			if (uiAction is LinkedUIAction linkedUIAction && linkedUIAction.Action is CasinoIdler.Action walletAction)
			{
				walletAction.Execute(playerWallet);

				linkedUIAction.SelectableUI.OnAction(walletAction.actionType, selector.SelectedPosition);

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
				if(activeAction is LinkedUIAction linkedUIAction)
					linkedUIAction.ActionButton.interactable = linkedUIAction.Action.CanExecute(playerWallet);
			}
		}
	}
}
