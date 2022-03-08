using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIRoom : MonoBehaviour
{
	private bool hasDependencies;
	protected Rect rect;
	protected string name;
	protected Cashier cashier;

	public void SetDependencies(Rect rect, string name, ref Cashier cashier)
	{
		this.rect = rect;
		this.name = name;
		this.cashier = cashier;

		hasDependencies = true;
	}

	private void OnGUI()
	{
		if (!hasDependencies)
			return;

		if (GUI.Button(rect, name))
		{
			OnClick();
		}
	}

	protected abstract void OnClick();
}
