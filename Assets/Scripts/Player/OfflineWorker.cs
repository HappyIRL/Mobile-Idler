using System;

public static class OfflineWorker
{
	public static double GetOfflineGeneratedAmount(DateTime? lastPlayed, uint productionRatePerSecond)
	{
		if (lastPlayed.HasValue)
		{
			uint offlineProductionAmount;

			uint offlineSeconds = (uint)(DateTime.Now - lastPlayed.Value).TotalSeconds;

			//28800 seconds are 8h
			if (offlineSeconds > 28800) offlineSeconds = 28800;

			if (offlineSeconds > 7200)
			{
				offlineProductionAmount = (uint) Math.Round((7200 * productionRatePerSecond + (offlineSeconds - 7200) * productionRatePerSecond * 0.2f) / 5.0) * 5;

				return offlineProductionAmount;
			}

			offlineProductionAmount = offlineSeconds * productionRatePerSecond;

			return offlineProductionAmount;
		}

		return 0;
	}
}
