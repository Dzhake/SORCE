﻿using RogueLibsCore;
using System;
using System.Collections.Generic;
using UnityEngine;
using SORCE;
using Random = UnityEngine.Random;
using Object = UnityEngine.Object;
using SORCE.Challenges;
using SORCE.Content.Challenges;
using SORCE.Localization;
using System.Linq;

namespace SORCE.Challenges.C_Wreckage
{
	public class DirtierDistricts
	{
		[RLSetup]
		static void Start()
		{
			const string name = nameof(DirtierDistricts);

			RogueLibs.CreateCustomUnlock(new MutatorUnlock(name, true)
			{
			})
				.WithName(new CustomNameInfo(
					"Wreckage - Dirtier Districts"))
				.WithDescription(new CustomNameInfo(
					"Now you don't just live in a disgusting dump, you play in a virtual one too!\n\n" +
					"- Spawns trash in public areas" +
					"- Piles of receipts next to ATMs"));
		}
	}
}