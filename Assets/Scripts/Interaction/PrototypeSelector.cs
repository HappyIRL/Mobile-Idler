using System.Collections.Generic;
using CasinoIdler;

public class PrototypeSelector : PlayerInputEventsBehaviour
{
	public ISelectable Selection { get; set; }
}

public interface ISelectable
{
	public ICollection<IAction> GetActions();
	public IReadOnlyList<ISelectable> SubSelections { get; }
	public string Name { get; }
}
