using System;
using UnityEngine;
using Zenject;

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

	[SerializeField] private Vector2MinMax cameraZoomClamp = new Vector2MinMax(-4, -11);
	[SerializeField] private float cameraMoveSpeed = 0.5f;

	private Camera camera;	
	private Transform cameraTransform;
	private Vector2MinMax cameraBoardersX = new Vector2MinMax(12f, 14f);
	private Vector2MinMax cameraBoardersY = new Vector2MinMax(7.5f, 9.5f);
	private float? touch01Magnitude; 

	private void Awake()
	{
		camera = GetComponent<Camera>();
		cameraTransform = camera.transform;
	}

	protected override void OnTouch1()
	{
		touch01Magnitude = (touch0Position - touch1Position).sqrMagnitude;
	}

	protected override void OnTouch1Cancelled()
	{
		touch01Magnitude = null;
	}

	protected override void OnTouch0DeltaChange(Vector2 delta)
	{
		if (activeTouches != 2)
		{
			MoveWithDelta(delta);
		}
	}

	protected override void OnTouch1DeltaChange(Vector2 delta)
	{
		if (activeTouches == 2)
		{
			Zoom(touch0Position, touch1Position);
		}
	}

	private void Zoom(Vector2 touch0, Vector2 touch1)
	{
		if (touch01Magnitude == null)
			return;

		Vector3 newCameraPosition = cameraTransform.position;

		float currentTouch01Magnitude = (touch0 - touch1).sqrMagnitude;

		float magnitudePercentDelta =  touch01Magnitude.Value / currentTouch01Magnitude;

		newCameraPosition.z *= magnitudePercentDelta;

		SetCameraPosition(newCameraPosition);

		touch01Magnitude = currentTouch01Magnitude;
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