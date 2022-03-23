using System;
using UnityEngine;

public class PrototypeSelector : MonoBehaviour
{
	private ISelectable selection;
	public ISelectable Selection => selection;

	public Action<ISelectable> SelectionChanged;
}

public interface ISelectable
{
	public CasinoIdler.Action[] GetActions();
}
