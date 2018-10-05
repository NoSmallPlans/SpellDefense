﻿using CocosSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpellDefense.Common.Entities
{

    public class CombatantSpawner : CCNode
    {
        List<CCPoint> RedSpawns;
        List<CCPoint> BlueSpawns;
        Random rnd = new Random();

        public CombatantSpawner()
        {
            this.RedSpawns = new List<CCPoint>();
            this.BlueSpawns = new List<CCPoint>();
            IsSpawning = true;
            TimeInbetweenSpawns = 1 / GameCoefficients.StartingCombatantPerSecond;
            // So that spawning starts immediately:
            timeSinceLastSpawn = TimeInbetweenSpawns;
        }


        public void CreateSpawnPts()
        {
            //TOOD remove +-10 magic numbers
            //TODO rename parent to something besides daddy... Thats weird
            UIcontainer daddy = (UIcontainer)this.Parent;
            float redX = daddy.minX + 10;
            float blueX = daddy.width - 10;
            float yBottom = daddy.height / 4;
            float yMid = daddy.height / 2;
            float yTop = daddy.height * .75f;


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

        /*
        public CCLayer Layer
        {
            get;
            set;
        }
        */
        

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

                Spawn(RedSpawns, Team.ColorChoice.RED);
                Spawn(BlueSpawns, Team.ColorChoice.BLUE);
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
        private void Spawn(List<CCPoint> spawns, Team.ColorChoice team)
        {
            //TODO - Fix me
            //Spawn both ranged and melee
            BasicRanged Combatant;
            
            int i = rnd.Next(0,3);
            Combatant = new BasicRanged(team);
            Combatant.PositionX = spawns[i].X;
            Combatant.PositionY = spawns[i].Y;
            if (CombatantSpawned != null)
            {
                CombatantSpawned(Combatant);
            }
            
        }



    }
}
