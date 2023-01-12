using System;

public static class OfflineWorker
{
	public static double GetOfflineGeneratedAmount(DateTime? lastPlayed, uint productionRate)
	{
		if (lastPlayed.HasValue)
		{
			uint offlineProductionAmount;

			uint offlineSeconds = (uint)(DateTime.Now - lastPlayed.Value).TotalSeconds;

			if (offlineSeconds > 28800) offlineSeconds = 28800;

			if (offlineSeconds > 7200)
			{
				offlineProductionAmount = (uint) Math.Round((7200 * productionRate + (offlineSeconds - 7200) * productionRate * 0.2f) / 5.0) * 5;

				return offlineProductionAmount;
			}

			offlineProductionAmount = offlineSeconds * productionRate;

			return offlineProductionAmount;
		}

		return 0;
	}
}
