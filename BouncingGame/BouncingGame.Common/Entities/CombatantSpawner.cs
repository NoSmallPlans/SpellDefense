﻿using CocosSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpellDefense.Common.Entities
{

    public class CombatantSpawner
    {
        List<CCPoint> RedSpawns;
        List<CCPoint> BlueSpawns;

        public CombatantSpawner()
        {
            this.RedSpawns = new List<CCPoint>();
            this.BlueSpawns = new List<CCPoint>();
            IsSpawning = true;
            TimeInbetweenSpawns = 1 / GameCoefficients.StartingCombatantPerSecond;
            // So that spawning starts immediately:
            timeSinceLastSpawn = TimeInbetweenSpawns;
        }

        //TODO: Combatant spawner doesn't really work without a layer
        //discuss making layer a required parameter on the constructor
        public void CreateSpawnPts()
        {

            /*
            float redX = 10;
            float blueX = Layer.ContentSize.Width-10;
            float yBottom = Layer.ContentSize.Height / 4;
            float yMid = Layer.ContentSize.Height / 2;
            float yTop = Layer.ContentSize.Height * .75f;
            */
            //TOOD remove +-10 magic numbers
            float redX = GameCoefficients.Battlefield.GetMinX() + 10;
            float blueX = GameCoefficients.Battlefield.GetWidth() - 10;
            float yBottom = GameCoefficients.Battlefield.GetHeight() / 4;
            float yMid = GameCoefficients.Battlefield.GetHeight() / 2;
            float yTop = GameCoefficients.Battlefield.GetHeight() * .75f;


            RedSpawns.Add(new CCPoint(redX, yBottom));
            RedSpawns.Add(new CCPoint(redX, yMid));
            RedSpawns.Add(new CCPoint(redX, yTop));

            BlueSpawns.Add(new CCPoint(blueX, yBottom));
            BlueSpawns.Add(new CCPoint(blueX, yMid));
            BlueSpawns.Add(new CCPoint(blueX, yTop));

        }

        float timeSinceLastSpawn;
        public float TimeInbetweenSpawns
        {
            get;
            set;
        }

        public string DebugInfo
        {
            get
            {
                string toReturn =
                    "Combatant per second: " + (1 / TimeInbetweenSpawns);

                return toReturn;
            }
        }

        public CCLayer Layer
        {
            get;
            set;
        }

        public Action<Combatant> CombatantSpawned;

        public bool IsSpawning
        {
            get;
            set;
        }



        public void Activity(float frameTime)
        {
            if (IsSpawning)
            {
                SpawningActivity(frameTime);

                SpawnReductionTimeActivity(frameTime);
            }
        }

        private void SpawningActivity(float frameTime)
        {
            timeSinceLastSpawn += frameTime;

            if (timeSinceLastSpawn > TimeInbetweenSpawns)
            {
                timeSinceLastSpawn -= TimeInbetweenSpawns;

                Spawn(RedSpawns, GameCoefficients.TeamColor.RED);
                Spawn(BlueSpawns, GameCoefficients.TeamColor.BLUE);
            }
        }

        private void SpawnReductionTimeActivity(float frameTime)
        {
            // This logic should increase how frequently Combatant appear, but it should do so
            // such that the # of Combatant/minute increases at a decreasing rate, otherwise the
            // game becomes impossibly difficult very quickly.
            var currentCombatantPerSecond = 1 / TimeInbetweenSpawns;

            var amountToAdd = frameTime / GameCoefficients.TimeForExtraCombatantPerSecond;

            var newCombatantPerSecond = currentCombatantPerSecond + amountToAdd;

            TimeInbetweenSpawns = 1 / newCombatantPerSecond;

        }

        // made public for debugging, may make it private later:
        private void Spawn(List<CCPoint> spawns, GameCoefficients.TeamColor team)
        {
            BasicMelee Combatant;

            if (Layer == null)
            {
                throw new InvalidOperationException("Need to set Layer before spawning");
            }

            for (int i = 0; i < spawns.Count; i++)
            {
                Combatant = new BasicMelee(team);
                Combatant.PositionX = spawns[i].X;
                Combatant.PositionY = spawns[i].Y;
                if (CombatantSpawned != null)
                {
                    CombatantSpawned(Combatant);
                }
            }
        }



    }
}
