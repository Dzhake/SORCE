using System;
using System.Collections.Generic;
using System.Linq;
using RogueLibsCore;
using SORCE.Challenges;
using BepInEx.Logging;
using SORCE.Logging;
using SORCE.Challenges.C_Buildings;

namespace SORCE.Challenges
{
	public static class ChallengeManager
	{
		private static readonly ManualLogSource logger = SORCELogger.GetLogger();
		private static GameController GC => GameController.gameController;

		private static readonly Dictionary<Type, ChallengeInfo> registeredChallenges = new();

		public static string GetActiveChallengeFromList(List<string> challengeList) =>
            challengeList.FirstOrDefault(c => GC.challenges.Contains(c));

		public static Type GetActiveChallengeFromList(List<Type> challengeList) =>
            challengeList.FirstOrDefault(c => GC.challenges.Contains(nameof(c)));

		public static ChallengeInfo GetChallengeInfo<ChallengeType>() =>
			GetChallengeInfo(typeof(ChallengeType));

		public static ChallengeInfo GetChallengeInfo(Type ChallengeType) =>
			registeredChallenges.GetValueOrDefault(ChallengeType);

		public static bool IsChallengeFromListActive(List<string> challengeList) =>
            challengeList.Any(c => GC.challenges.Contains(c));

		public static T SetCancellations<T>(this T wrapper, IEnumerable<string> cancellations) where T : UnlockWrapper
		{
			if (wrapper.Unlock.cancellations == null)
				wrapper.Unlock.cancellations = new List<string>();
			
			wrapper.Unlock.cancellations.Clear();
			wrapper.Unlock.cancellations.AddRange(cancellations);
			return wrapper;
		}
	}
}