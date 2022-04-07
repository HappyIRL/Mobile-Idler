using System.Collections.Generic;
using CasinoIdler;

public class PrototypeSelector : PlayerInputEventsBehaviour
{
	public ISelectable Selection { get; private set; }
	public ISelectable OldSelection { get; private set; }

	public void SetSelection(ISelectable selectable)
	{
		if (Selection != null)
			Selection.Unselect -= OnUnselect;

		if (selectable != null)
		{
			selectable.Unselect += OnUnselect;
		}

		OldSelection = Selection;
		Selection = selectable;
	}

	private void OnUnselect()
	{
		SetSelection(null);
	}
}

public interface ISelectable
{
	public System.Action Unselect { get; set; }
	public ICollection<IAction> GetActions();
	public IReadOnlyList<ISelectable> SubSelections { get; }
	public string Name { get; }
}