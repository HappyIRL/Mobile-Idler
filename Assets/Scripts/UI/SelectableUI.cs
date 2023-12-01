
using CasinoIdler;
using UnityEngine;

namespace Assets.Scripts.UI
{
	//remove unregister, implement constructor for all bases, call DrawAll and RegisterUiFields, make drawall abstract
	public abstract class SelectableUI
	{
		public abstract ISelectable Selectable { get; }
		protected abstract void RegisterUiField();
		protected abstract void UnregisterUIFields();
		public abstract void OnAction(ActionType actionType, Vector2Int postion);
	}
}
