using System;
using System.Collections.Generic;
using System.Xml;
using UnityEditor;
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

	[SerializeField] private Vector2MinMax cameraZoomClamp = new Vector2MinMax(4, 120);
	[SerializeField] private float cameraMoveSpeed = 0.5f;
	[SerializeField] private float cameraZoomSpeed = 80f;

	private Camera camera;
	private Transform cameraTransform;

	private Vector3 cameraPosition;
	private Vector2 touch0Delta;
	private Vector2MinMax cameraBoarders = new Vector2MinMax(0, 40);

	private void Awake()
	{
		camera = GetComponent<Camera>();
		cameraTransform = camera.transform;
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		selector.NewNullableSelection += OnNewSelection;
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

	private void Zoom(Vector2 x, Vector2 y)
	{
		Vector3 newCameraPosition = cameraTransform.position;

		//facing towards - zoom out
		if (x.x > 0 && y.x < 0)
		{
			newCameraPosition.y -= cameraZoomSpeed * Time.deltaTime;
		}
		//facing away - zoom in
		else if (x.x < 0 && y.x > 0)
		{
			newCameraPosition.y += cameraZoomSpeed * Time.deltaTime;
		}

		newCameraPosition.y = Mathf.Clamp(newCameraPosition.y, cameraZoomClamp.Min, cameraZoomClamp.Max);

		SetCameraPosition(newCameraPosition);
	}

	private Vector3 GetClampedPosition(Vector3 position)
	{
		position.x = Mathf.Clamp(position.x, cameraBoarders.Min, cameraBoarders.Max);
		position.z = Mathf.Clamp(position.z, cameraBoarders.Min, cameraBoarders.Max);
		position.y = Mathf.Clamp(position.y, cameraZoomClamp.Min, cameraZoomClamp.Max);
		return position;
	}

	private void SetCameraPosition(Vector3 newPosition)
	{
		cameraTransform.position = GetClampedPosition(newPosition);
	}

	private void MoveWithDelta(Vector2 delta)
	{
		cameraPosition = cameraTransform.position;
		float zoomScaler = cameraPosition.y / 6;
		cameraPosition.x -=  delta.x * Time.deltaTime * cameraMoveSpeed * zoomScaler;
		cameraPosition.z -= delta.y * Time.deltaTime * cameraMoveSpeed * zoomScaler;
		SetCameraPosition(cameraPosition);
	}

	public void MoveToHorizontal(Vector3 position)
	{
		position.y = cameraTransform.position.y;
		SetCameraPosition(position);
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		selector.NewNullableSelection -= OnNewSelection;
	}
}