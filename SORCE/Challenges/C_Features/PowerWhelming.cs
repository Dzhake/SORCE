﻿using RogueLibsCore;
using System;
using System.Collections.Generic;
using UnityEngine;
using SORCE;
using Random = UnityEngine.Random;
using Object = UnityEngine.Object;
using SORCE.Challenges;
using SORCE.Content.Challenges;

namespace SORCE.Challenges.C_Features
{
	public class PowerWhelming
	{
		[RLSetup]
		static void Start()
		{
			const string name = nameof(PowerWhelming);

			UnlockBuilder unlockBuilder = RogueLibs.CreateCustomUnlock(new MutatorUnlock(name, true));

			ChallengeManager.RegisterChallenge<PowerWhelming>(new ChallengeInfo(name, unlockBuilder));
		}
	}
}