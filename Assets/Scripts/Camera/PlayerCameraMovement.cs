using System;
using UnityEngine;

public struct Vector2MinMax
{
	public float Min;
	public float Max;

	public Vector2MinMax(float min, float max)
	{
		Min = min;
		Max = max;
	}
}

public class PlayerCameraMovement : PlayerInputEventsBehaviour
{
	/// <summary>
	/// Camera clamped in a square
	/// clamped square gets bigger when you zoom in and smaller when you zoom out
	/// 2 fingers zooms the camera in and out
	/// 1 finger moves the camera
	/// 
	/// </summary>

	[Zenject.Inject] private Selector selector;

	[SerializeField] private Vector2MinMax cameraZoomClamp = new Vector2MinMax(-4, -11);
	[SerializeField] private float cameraMoveSpeed = 0.5f;
	[SerializeField] private float cameraZoomSpeed = 20f;

	private Camera camera;	
	private Transform cameraTransform;
	private Vector2 touch0Delta;
	private Vector2MinMax cameraBoardersX = new Vector2MinMax(12f, 14f);
	private Vector2MinMax cameraBoardersY = new Vector2MinMax(7.5f, 9.5f);

	private void Awake()
	{
		camera = GetComponent<Camera>();
		cameraTransform = camera.transform;
	}

	protected override void OnTouch0DeltaChange(Vector2 delta)
	{
		touch0Delta = delta;

		if (activeTouches != 2)
		{
			MoveWithDelta(delta);
		}
	}

	protected override void OnTouch1DeltaChange(Vector2 delta)
	{
		if (activeTouches == 2)
		{
			if(IsABiggerByX(touch0Position, touch1Position))
				Zoom(touch0Delta, delta);
			else
				Zoom(delta, touch0Delta);
		}
	}

	private void OnNewSelection(Transform transform)
	{
		if(transform != null)
			MoveToHorizontal(transform.position);
	}

	private bool IsABiggerByX(Vector2 a, Vector2 b)
	{
		if (a.x > b.x)
			return true;

		return false;
	}

	[NaughtyAttributes.Button("Out")]
	private void ZoomIn()
	{
		Zoom(new Vector2(0,0), new Vector2(1,0));
	}

	[NaughtyAttributes.Button("In")]
	private void ZoomOut()
	{
		Zoom(new Vector2(0, 0), new Vector2(-1, 0));
	}

	private void Zoom(Vector2 x, Vector2 y)
	{
		Vector3 newCameraPosition = cameraTransform.position;

		//facing towards - zoom out

		if (x.x == 0 && y.x == 0)
			return;

		if (x.x >= 0 && y.x <= 0)
		{
			newCameraPosition.z += cameraZoomSpeed * Time.deltaTime;
		}
		//facing away - zoom in
		else if (x.x <= 0 && y.x >= 0)
		{
			newCameraPosition.z -= cameraZoomSpeed * Time.deltaTime;
		}

		SetCameraPosition(newCameraPosition);
	}

	private Vector3 GetClampedPosition(Vector3 position)
	{
		position.x = Mathf.Clamp(position.x, cameraBoardersX.Min - (cameraTransform.position.z + 11), cameraBoardersX.Max + (cameraTransform.position.z + 11));
		position.y = Mathf.Clamp(position.y, cameraBoardersY.Min - ((cameraTransform.position.z + 11) * 0.7f), cameraBoardersY.Max + ((cameraTransform.position.z + 11) * 0.7f));
		position.z = Mathf.Clamp(position.z, cameraZoomClamp.Max, cameraZoomClamp.Min);

		return position;
	}

	private void SetCameraPosition(Vector3 newPosition)
	{
		cameraTransform.position = GetClampedPosition(newPosition);
	}

	private void MoveWithDelta(Vector2 delta)
	{
		Vector3 cameraPosition = cameraTransform.position;
		float zoomScaler = cameraPosition.z / 6;
		cameraPosition.x +=  delta.x * Time.deltaTime * cameraMoveSpeed * zoomScaler;
		cameraPosition.y += delta.y * Time.deltaTime * cameraMoveSpeed * zoomScaler;

		SetCameraPosition(cameraPosition);
	}

	public void MoveToHorizontal(Vector3 position)
	{
		position.z = cameraTransform.position.z;
		SetCameraPosition(position);
	}
}