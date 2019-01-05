﻿using CocosSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SpellDefense.Common.GodClass;

namespace SpellDefense.Common.Entities
{

    public class CombatantSpawner : CCNode
    {
        CCPoint redSpawn;
        CCPoint blueSpawn;
        TeamColor teamColor;

        //Units that will spawn every spawn event
        //Firt list, index 0, represents permanent spawns
        //Index 1...count represents current and future temporary spawns
        List<List<Unit>> spawnLists;  

        struct Unit
        {
            public int num;
            public string combatantType;
        }

        public CombatantSpawner(TeamColor teamColor)
        {
            IsSpawning = true;
            TimeInbetweenSpawns = 4;
            // So that spawning starts immediately:
            timeSinceLastSpawn = TimeInbetweenSpawns;
            this.teamColor = teamColor;
            InitSpawnLists();
        }

        private void InitSpawnLists()
        {
            spawnLists = new List<List<Unit>>();
            AddSpawn(3, 0, "BasicMelee");        
        }

        private Combatant GetCombatant(string combatantType)
        {
            switch(combatantType)
            {
                case "BasicRanged":
                    return new BasicRanged(teamColor);
                case "BasicMelee":
                    return new BasicMelee(teamColor);
                default:
                    return new BasicMelee(teamColor);
            }
        }

        public void AddSpawn(int num, int spawns, string combatantType)
        {
            //Check to see if list exists
            //Check to see if combatant type exists
            //Add to list
            Unit unit;
            for(int i = 0; i <= spawns; i++)
            {
                if(spawnLists.Count <= i)
                {
                    spawnLists.Add(new List<Unit>());
                }
                if (spawnLists[i].Any(q => q.combatantType == combatantType))
                {
                    unit = spawnLists[i].First(q => q.combatantType == combatantType);
                    unit.num += num;
                }
                else
                {
                    unit = new Unit();
                    unit.num = num;
                    unit.combatantType = combatantType;
                    spawnLists[i].Add(unit);
                }
            }
        }

        public void CreateSpawnPts()
        {
            //TOOD remove +-10 magic numbers
            //TODO rename parent to something besides daddy... Thats weird
            UIcontainer daddy = GodClass.battlefield;
            float redX = daddy.minX + 10;
            float blueX = daddy.width - 10;
            float yMid = daddy.height / 2;

            redSpawn = new CCPoint(redX, yMid);
            blueSpawn = new CCPoint(blueX, yMid);
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

                if(teamColor == TeamColor.RED)
                    Spawn(redSpawn);
                else
                    Spawn(blueSpawn);
            }
        }

        private void SpawnReductionTimeActivity(float frameTime)
        {
            // This logic should increase how frequently Combatant appear, but it should do so
            // such that the # of Combatant/minute increases at a decreasing rate, otherwise the
            // game becomes impossibly difficult very quickly.
            var currentCombatantPerSecond = 1 / TimeInbetweenSpawns;

            var amountToAdd = frameTime / GodClass.TimeForExtraCombatantPerSecond;

            var newCombatantPerSecond = currentCombatantPerSecond + amountToAdd;

            TimeInbetweenSpawns = 1 / newCombatantPerSecond;

        }

        // made public for debugging, may make it private later:
        private void Spawn(CCPoint spawnPoint)
        {
            int rows = 3;
            int curRow = 1;
            int curCol = 1;
            //TODO remove hard coding on spacing, change it to sprite width/height
            float heightSpacing = 40.0f;
            float widthSpacing = 40.0f;

            for (int i = 0; i < (spawnLists.Count > 1 ? 2 : 1); i++)
            {
                foreach (Unit unit in spawnLists[i])
                {
                    for (int n = 0; n < unit.num; n++)
                    {
                        Combatant c = GetCombatant(unit.combatantType);
                        c.Position = spawnPoint;
                        c.PositionX += curCol * widthSpacing;
                        c.PositionY += curRow * heightSpacing;
                        curRow++;
                        if (curRow > rows)
                        {
                            curRow = 1;
                            curCol++;
                        }
                        CombatantSpawned(c);
                    }
                }
            }
            if (spawnLists.Count > 1)
            {
                spawnLists.RemoveAt(1);
            }
        }
    }
}
