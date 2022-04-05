using System.Collections.Generic;
using CasinoIdler;

public class PrototypeSelector : PlayerInputEventsBehaviour
{
	public ISelectable Selection { get; private set; }

	public void SetSelectable(ISelectable selectable)
	{
		if (Selection != null)
			Selection.Unselect -= OnUnselect;

		Selection = selectable;
		selectable.Unselect += OnUnselect;
	}

	private void OnUnselect()
	{
		Selection.Unselect -= OnUnselect;
		Selection = null;
	}
}

public interface ISelectable
{
	public System.Action Unselect { get; set; }
	public ICollection<IAction> GetActions();
	public IReadOnlyList<ISelectable> SubSelections { get; }
	public string Name { get; }
}
