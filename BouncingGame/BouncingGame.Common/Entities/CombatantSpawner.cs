using CocosSharp;
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
        List<List<Squad>> spawnLists;  

        class Squad
        {
            public int qty;
            public string combatantType;
        }

        public CombatantSpawner(TeamColor teamColor)
        {
            IsSpawning = false;
            TimeInbetweenSpawns = 16;
            // So that spawning starts immediately:
            timeSinceLastSpawn = TimeInbetweenSpawns;
            this.teamColor = teamColor;
            InitSpawnLists();
        }

        private void InitSpawnLists()
        {
            spawnLists = new List<List<Squad>>();
        }

        public void AddSpawn(int qty, int spawns, string combatantType)
        {
            //Check to see if list exists
            //Check to see if combatant type exists
            //Add to list
            Squad squad;
            for(int i = spawns > 0 ? 1 : 0; i <= spawns; i++)
            {
                if(spawnLists.Count <= i)
                {
                    spawnLists.Add(new List<Squad>());
                }
                if (spawnLists[i].Any(q => q.combatantType == combatantType))
                {
                    squad = spawnLists[i].First(q => q.combatantType == combatantType);
                    squad.qty += qty;
                }
                else
                {
                    squad = new Squad();
                    squad.qty = qty;
                    squad.combatantType = combatantType;
                    spawnLists[i].Add(squad);
                }
            }
        }

        public void CreateSpawnPts()
        {
            //TOOD remove +-10 magic numbers
            //TODO rename parent to something besides daddy... Thats weird
            UIcontainer daddy = GodClass.battlefield;
            float redX = 0;
            float blueX = 9;
            float yMid = 5;

            redSpawn = new CCPoint(redX, 1);
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
            }
        }

        private void SpawningActivity(float frameTime)
        {
            timeSinceLastSpawn += frameTime;

            if (timeSinceLastSpawn > TimeInbetweenSpawns)
            {
                timeSinceLastSpawn = 0;

                if(teamColor == TeamColor.RED)
                    Spawn(redSpawn);
                else
                    Spawn(blueSpawn);
            }
        }

        public void HandleTurnTimeReached()
        {
            if (teamColor == TeamColor.RED)
                Spawn(redSpawn);
            else
                Spawn(blueSpawn);
        }

        private void Spawn(CCPoint spawnPoint)
        {
            for (int i = 0; i < (spawnLists.Count > 1 ? 2 : 1); i++)
            {
                foreach (Squad squad in spawnLists[i])
                {
                    for (int n = 0; n < squad.qty; n++)
                    {
                        string unitJson = GodClass.UnitLibrary[squad.combatantType];
                        Combatant c = new BasicMelee(teamColor, unitJson);
                        c.Position = spawnPoint;
                        if(teamColor == TeamColor.RED)
                        {
                            GodClass.gridManager.PlaceGamePiece(c, redSpawn);
                        } else
                        {
                            GodClass.gridManager.PlaceGamePiece(c, blueSpawn);
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
