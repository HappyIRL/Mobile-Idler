using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UiButton : MonoBehaviour
{
	private bool hasDependencies;
	protected Rect rect;
	protected string name;

	//does ref even do anything here?
	public virtual void Init(Rect rect, string name)
	{
		this.rect = rect;
		this.name = name;

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
