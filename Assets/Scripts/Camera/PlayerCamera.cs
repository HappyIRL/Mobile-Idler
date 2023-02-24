using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
	private Camera camera;
	private Transform cameraTransform;

	private void Awake()
	{
		camera = GetComponent<Camera>();
		cameraTransform = camera.transform;
	}

	public Vector3 ScreenPointToWorldPos(Vector2 point)
	{
		return camera.ScreenToWorldPoint(new Vector3(point.x, point.y, cameraTransform.position.z * -1));
	}
}