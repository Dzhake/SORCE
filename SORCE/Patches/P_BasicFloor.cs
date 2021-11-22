﻿using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using RogueLibsCore;
using Random = UnityEngine.Random;
using System.Collections;
using System.Reflection;
using System;
using SORCE.Logging;
using SORCE.Challenges;

namespace SORCE.Patches
{
	[HarmonyPatch(declaringType: typeof(BasicFloor))]
	public static class P_BasicFloor
	{
		private static readonly ManualLogSource logger = SORCELogger.GetLogger();
		public static GameController GC => GameController.gameController;

		/// <summary>
		///		Floor Interiors, 
		/// </summary>
		/// <param name="spawner"></param>
		/// <param name="floorName"></param>
		/// <param name="myPos"></param>
		/// <param name="myScale"></param>
		/// <param name="startingChunkReal"></param>
		/// <returns></returns>
		[HarmonyPrefix, HarmonyPatch(methodName: nameof(BasicFloor.Spawn), argumentTypes: new[] { typeof(SpawnerBasic), typeof(string), typeof(Vector2), typeof(Vector2), typeof(Chunk) })]
		public static bool Spawn_Prefix(SpawnerBasic spawner, ref string floorName, Vector2 myPos, Vector2 myScale, Chunk startingChunkReal)
		{
			if (LevelGenTools.GetActiveFloorMod() != null)
			{
				if (vFloor.Natural.Contains(floorName))
				{
					if (GC.challenges.Contains(cChallenge.GreenLiving))
						floorName = vFloor.Grass;
					else if (GC.challenges.Contains(cChallenge.SpelunkyDory))
						floorName = vFloor.CaveFloor;
				}
				else if (vFloor.Rugs.Contains(floorName))
				{
					if (GC.challenges.Contains(cChallenge.DiscoCityDanceoff)) // Overrides some non-exclusive challenges
						floorName = vFloor.CasinoFloor;
					else if (GC.challenges.Contains(cChallenge.CityOfSteel))
						floorName = vFloor.MetalPlates;
					else if (GC.challenges.Contains(cChallenge.GreenLiving))
						floorName = vFloor.Grass;
					else if (GC.challenges.Contains(cChallenge.Panoptikopolis))
						floorName = vFloor.ClearFloor;
					else if (GC.challenges.Contains(cChallenge.SpelunkyDory))
						floorName = vFloor.DirtFloor;
				}
				else if (vFloor.Constructed.Contains(floorName))
				{
					if (GC.challenges.Contains(cChallenge.DiscoCityDanceoff)) // Overrides some non-exclusive challenges
						floorName = vFloor.BathroomTile;
					else if (GC.challenges.Contains(cChallenge.CityOfSteel))
						floorName = vFloor.MetalFloor;
					else if (GC.challenges.Contains(cChallenge.GreenLiving))
						floorName = vFloor.DirtFloor;
					else if (GC.challenges.Contains(cChallenge.Panoptikopolis))
						floorName = vFloor.CleanTiles;
					else if (GC.challenges.Contains(cChallenge.ShantyTown))
						floorName = vFloor.DrugDenFloor;
					else if (GC.challenges.Contains(cChallenge.SpelunkyDory))
						floorName = vFloor.CaveFloor;
				}
				else if (vFloor.Raised.Contains(floorName))
				{
					if (GC.challenges.Contains(cChallenge.DiscoCityDanceoff)) // Overrides some non-exclusive challenges
						floorName = vFloor.DanceFloorRaised;
					else if (GC.challenges.Contains(cChallenge.CityOfSteel))
						floorName = vFloor.SolidPlates;
					else if (GC.challenges.Contains(cChallenge.GreenLiving))
						floorName = vFloor.CaveFloor;
					else if (GC.challenges.Contains(cChallenge.Panoptikopolis))
						floorName = vFloor.CleanTilesRaised;
					else if (GC.challenges.Contains(cChallenge.ShantyTown))
						floorName = vFloor.DirtyTiles;
					else if (GC.challenges.Contains(cChallenge.SpelunkyDory))
						floorName = vFloor.Grass;
				}
			}

			return true;
		}
	}
}
