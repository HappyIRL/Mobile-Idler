using System;
using UnityEngine;

public class Selector : PlayerInputEventsBehaviour
{
	[Zenject.Inject] private PlayerCamera playerCamera;

	public Action<Transform> NewNullableSelection;

	private Transform selected;

	protected override void OnTouch0Tap(Vector2 touchPos)
	{
		RaycastHit? hit = playerCamera.MouseToWorldRay(touchPos);

		if (hit == null)
		{
			NewNullableSelection?.Invoke(null);
			return;
		}

		SelectNew(hit.Value.transform);
	}

	private void SelectNew(Transform transform)
	{
		NewNullableSelection?.Invoke(transform);
	}
}

