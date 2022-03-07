using UnityEngine;

public class DeviceOrientationHandler : MonoBehaviour
{
	private const float ORIENTATION_CHECK_INTERVAL = 1f;

	private float nextOrientationCheckTime;

	private void Start()
	{
		Screen.orientation = ScreenOrientation.LandscapeLeft;
	}
	private void Update()
	{

		if (Time.realtimeSinceStartup >= nextOrientationCheckTime)
		{
			DeviceOrientation orientation = Input.deviceOrientation;

			switch (orientation)
			{
				case DeviceOrientation.LandscapeLeft:
					Screen.orientation = ScreenOrientation.LandscapeLeft;
					break;
				case DeviceOrientation.LandscapeRight:
					Screen.orientation = ScreenOrientation.LandscapeRight;
					break;
			}
			nextOrientationCheckTime = Time.realtimeSinceStartup + ORIENTATION_CHECK_INTERVAL;
		}
	}
}

