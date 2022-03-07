using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
	private Camera camera;

	private void Awake()
	{
		camera = GetComponent<Camera>();
	}

	public RaycastHit? MouseToWorldRay(Vector2 mousePos)
	{
		Ray ray = camera.ScreenPointToRay(mousePos);

		if (Physics.Raycast(ray, out RaycastHit hit))
		{
			return hit;
		}

		return null;
	}
}