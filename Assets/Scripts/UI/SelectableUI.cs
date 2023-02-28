
using CasinoIdler;
using UnityEngine;

namespace Assets.Scripts.UI
{
	//the interface might not be needed and is just double next to the abstract class
	public abstract class SelectableUI
	{
		public abstract ISelectable Selectable { get; }
		protected abstract void RegisterUiField();
		protected abstract void UnregisterUiField();
		public abstract void OnAction(IAction action);
	}
}
