using UnityEngine;

public class PlayerInputEventsBehaviour : MonoBehaviour
{
	[Zenject.Inject] private PlayerInputBroadcast playerInputBroadcast;

	protected int activeTouches => playerInputBroadcast.ActiveTouches;
	protected Vector2 touch0Position => playerInputBroadcast.Touch0Position;
	protected Vector2 touch1Position => playerInputBroadcast.Touch1Position;

	protected virtual void OnEnable()
	{
		playerInputBroadcast.Touch0Tap += OnTouch0Tap;
		playerInputBroadcast.Touch0DeltaChange += OnTouch0DeltaChange;
		playerInputBroadcast.Touch1DeltaChange += OnTouch1DeltaChange;
		playerInputBroadcast.Touch1 += OnTouch1;
		playerInputBroadcast.Touch1Cancelled += OnTouch1Cancelled;
	}

	protected virtual void OnTouch0Tap(Vector2 value)
	{

	}

	protected virtual void OnTouch1()
	{

	}

	protected virtual void OnTouch1Cancelled()
	{
		
	}

	protected virtual void OnTouch0DeltaChange(Vector2 value)
	{

	}

	protected virtual void OnTouch1DeltaChange(Vector2 value)
	{

	}

	protected virtual void OnDisable()
	{
		playerInputBroadcast.Touch0Tap -= OnTouch0Tap;
		playerInputBroadcast.Touch0DeltaChange -= OnTouch0DeltaChange;
		playerInputBroadcast.Touch1DeltaChange -= OnTouch1DeltaChange;
		playerInputBroadcast.Touch1 -= OnTouch1;
	}
}
