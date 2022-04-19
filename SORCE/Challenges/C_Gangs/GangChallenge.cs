﻿using BepInEx.Logging;
using RogueLibsCore;
using SORCE.Logging;
using SORCE.MapGenUtilities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static SORCE.Localization.NameLists;

namespace SORCE.Challenges.C_Gangs
{
    public abstract class GangChallenge : MutatorUnlock
    {
        public GangChallenge(string name) : base(name, true) { }

        public abstract string LeaderAgent { get; }
        public abstract string[] MiddleAgents { get; } // Can be in order, or a random pool?
        public abstract string LastAgent { get; }

        public abstract bool AlwaysRun { get; }
        public abstract bool MustBeGuilty { get; }

        public abstract int GangSize { get; }
        public abstract int TotalSpawns { get; }

        public abstract string Relationship { get; }
    }

    public static class GangChallengeTools
    {
		private static readonly ManualLogSource logger = SORCELogger.GetLogger();
		public static GameController GC => GameController.gameController;

		public static int GangTotalCount =>
            (int)(Random.Range(12, 24) * LevelSize.ChunkCountRatio);
        public static int GangSize =>
            Random.Range(4, 8);

		public static void Spawner_Main()
		{
			foreach (GangChallenge challenge in RogueFramework.Unlocks.OfType<GangChallenge>().Where(c => c.IsEnabled))
				SpawnGangs(challenge);
		}

		public static void SpawnGangs(GangChallenge challenge) =>
			SpawnGangs(challenge.LeaderAgent, challenge.MiddleAgents, challenge.LastAgent, challenge.TotalSpawns, challenge.GangSize, challenge.Relationship, challenge.AlwaysRun, challenge.MustBeGuilty);
		public static void SpawnGangs(string leaderAgent, string[] middleAgents, string lastAgent, int totalSpawns = 0, int gangSize = 0, string relationship = VRelationship.Neutral, bool alwaysRun = false, bool mustBeGuilty = true)
		{
			if (totalSpawns == 0)
				totalSpawns = GangTotalCount;

			if (gangSize == 0)
				gangSize = GangSize;

			List<Agent> spawnedAgentList = new List<Agent>();
			Agent playerAgent = GC.playerAgent;
			//playerAgent.gangStalking = Agent.gangCount;
			Vector2 pos = Vector2.zero;
			int middleAgentIndex = 0;

			totalSpawns = (int)(totalSpawns * LevelSize.ChunkCountRatio);

			for (int i = 0; i < totalSpawns; i++)
			{
				int attempts = 0;
				string agentType;

				if (i % gangSize == 0) // First
				{
					Agent.gangCount++;

					while ((pos == Vector2.zero || Vector2.Distance(pos, GC.playerAgent.tr.position) < 20f) && attempts < 300)
					{
						pos = GC.tileInfo.FindRandLocationGeneral(0.32f);
						attempts++;
					}

					agentType = leaderAgent;
					middleAgentIndex = 0;
				}
				else if (i % gangSize == gangSize - 1) // Last
                {
					pos = GC.tileInfo.FindLocationNearLocation(pos, null, 0.32f, 1.28f, true, true);
					agentType = lastAgent;
					middleAgentIndex = 0;
				}
                else // Middle
                {
					pos = GC.tileInfo.FindLocationNearLocation(pos, null, 0.32f, 1.28f, true, true);
					agentType = middleAgents[middleAgentIndex++];

					if (middleAgentIndex > middleAgents.Count() - 1)
						middleAgentIndex = 0;
				}

				if (pos != Vector2.zero)
				{
					Agent agent = GC.spawnerMain.SpawnAgent(pos, null, agentType);
					agent.movement.RotateToAngleTransform((float)Random.Range(0, 360));
					agent.gang = Agent.gangCount;
					agent.modLeashes = 0;
					agent.alwaysRun = alwaysRun;
					agent.wontFlee = true;
					agent.agentActive = true;
					//agent.statusEffects.AddStatusEffect("InvisiblePermanent");
					agent.oma.mustBeGuilty = mustBeGuilty;
					spawnedAgentList.Add(agent);

					if (spawnedAgentList.Count > 1)
						for (int j = 0; j < spawnedAgentList.Count; j++)
							if (spawnedAgentList[j] != agent)
							{
								agent.relationships.SetRelInitial(spawnedAgentList[j], nameof(relStatus.Aligned));
								spawnedAgentList[j].relationships.SetRelInitial(agent, nameof(relStatus.Aligned));
							}

					agent.relationships.SetRel(playerAgent, relationship);
					playerAgent.relationships.SetRel(agent, relationship);

					switch (relationship.ToString())
					{
						case nameof(relStatus.Annoyed):
							agent.relationships.SetRelHate(playerAgent, 1);
							playerAgent.relationships.SetRelHate(agent, 1);
							break;
						case nameof(relStatus.Hostile):
							agent.relationships.SetRelHate(playerAgent, 5);
							playerAgent.relationships.SetRelHate(agent, 5);
							break;
					}

					agent.SetDefaultGoal(VAgentGoal.WanderLevel);
				}

				if (i % gangSize == gangSize - 1)
					pos = Vector2.zero;
			}
		}
	}
}