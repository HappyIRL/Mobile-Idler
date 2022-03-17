using UnityEngine;

public class PrototypeSelector : MonoBehaviour
{
	private ISelectable selection = new CasinoIdler.DummySelectable();
	public ISelectable Selected => selection;
}

public interface ISelectable
{
	public CasinoIdler.Action[] GetActions();
}
