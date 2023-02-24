using System.Collections.Generic;
using CasinoIdler;
using UnityEngine;
using Action = System.Action;

public class Selector : PlayerInputEventsBehaviour
{
	[Zenject.Inject] private PlayerCamera playerCamera;

	public ISelectable Selection { get; private set; }
	public ISelectable OldSelection { get; private set; }

	public void SetSelection(ISelectable selectable)
	{

		OldSelection = Selection;
		Selection = selectable;
	}

	protected override void OnTouch0Tap(Vector2 touchPos)
	{
		Debug.Log(playerCamera.ScreenPointToWorldPos(touchPos));
	}
}

public interface ISelectable
{
	public Action InternalStructureChanged { get; set; }
	public ICollection<IAction> GetActions();
	public IReadOnlyList<ISelectable> SubSelections { get; }
	public string Name { get; }

}

