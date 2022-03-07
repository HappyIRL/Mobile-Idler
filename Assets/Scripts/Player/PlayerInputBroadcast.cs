using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerInputBroadcast : MonoBehaviour
{
	private IdleCasinoGame inputAction;
	private IdleCasinoGame.PlayerActions playerActions;

	public Action<Vector2> Touch0Tap;
	public Action<Vector2> Touch0DeltaChange;
	public Action<Vector2> Touch1DeltaChange;
	public Action Touch1;

	public Vector2 Touch0Position => playerActions.Touch0Position.ReadValue<Vector2>();
	public Vector2 Touch1Position => playerActions.Touch1Position.ReadValue<Vector2>();


	//EnhancedTouchSupport has this, needs to be enabled tho with overhead.
	private int activeTouches;
	public int ActiveTouches => activeTouches;

	private void Awake()
	{
		inputAction = new IdleCasinoGame();
		playerActions = inputAction.Player;
	}

	private void OnEnable()
	{
		playerActions.Enable();
		playerActions.Touch0Tap.started += OnTouch0Tap;
		playerActions.Touch0.started += OnTouch0;
		playerActions.Touch1.started += OnTouch1;
		playerActions.Touch0.canceled += OnTouch0Cancelled;
		playerActions.Touch1.canceled += OnTouch1Cancelled;
		playerActions.Touch0Delta.started += OnTouch0DeltaChange;
		playerActions.Touch1Delta.started += OnTouch1DeltaChange;
	}

	private bool IsPointerOverUIObject()
	{
		PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
		eventDataCurrentPosition.position = playerActions.Touch0Position.ReadValue<Vector2>();
		List<RaycastResult> results = new List<RaycastResult>();
		EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
		return results.Count > 0;
	}


	private void OnTouch0Tap(InputAction.CallbackContext obj)
	{
		if (IsPointerOverUIObject()) return;

		Vector2 touch0Pos = playerActions.Touch0Position.ReadValue<Vector2>();

		Touch0Tap?.Invoke(touch0Pos);
	}

	private void OnTouch0(InputAction.CallbackContext obj)
	{
		if (IsPointerOverUIObject()) return;
		activeTouches++;
	}

	private void OnTouch1(InputAction.CallbackContext obj)
	{
		if (IsPointerOverUIObject()) return;
		activeTouches++;
		Touch1?.Invoke();
	}

	private void OnTouch0Cancelled(InputAction.CallbackContext obj)
	{
		if(activeTouches > 0)
			activeTouches--;
	}
	private void OnTouch1Cancelled(InputAction.CallbackContext obj)
	{
		if (activeTouches > 0)
			activeTouches--;
	}

	private void OnTouch0DeltaChange(InputAction.CallbackContext obj)
	{
		if (activeTouches == 0) return;
		Vector2 delta = obj.ReadValue<Vector2>();
		Touch0DeltaChange?.Invoke(delta);
	}

	private void OnTouch1DeltaChange(InputAction.CallbackContext obj)
	{
		if (activeTouches == 0) return;
		Vector2 delta = obj.ReadValue<Vector2>();
		Touch1DeltaChange?.Invoke(delta);
	}

	private void OnDisable()
	{
		playerActions.Touch0Tap.started -= OnTouch0Tap;
		playerActions.Touch0.started -= OnTouch0;
		playerActions.Touch1.started -= OnTouch1;
		playerActions.Touch0.canceled -= OnTouch0Cancelled;
		playerActions.Touch1.canceled -= OnTouch1Cancelled;
		playerActions.Touch0Delta.started -= OnTouch0DeltaChange;
		playerActions.Touch1Delta.started -= OnTouch1DeltaChange;
		playerActions.Disable();
	}
}

