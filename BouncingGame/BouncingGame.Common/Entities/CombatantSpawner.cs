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
            this.teamColor = teamColor;
            InitSpawnLists();
        }

        private void InitSpawnLists()
        {
            spawnLists = new List<List<Squad>>();
            AddSpawn(3, 0, "soldier");
        }

        public void AddSpawn(int qty, int spawns, string combatantType)
        {
            //Check to see if list exists
            //Check to see if combatant type exists
            //Add to list
            Squad squad;
            for(int i = 0; i <= spawns; i++)
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
            float redX = daddy.minX + 10;
            float blueX = daddy.width - 10;
            float yMid = daddy.height / 2;

            redSpawn = new CCPoint(redX, yMid);
            blueSpawn = new CCPoint(blueX, yMid);
        }
        
        public Action<Combatant> CombatantSpawned;

        /*
        public static void Activity(float frameTime)
        {
            if (isSpawning)
            {
                SpawningActivity(frameTime);
            }
        }
        */

        public void HandleTurnTimeReached()
        {
            if (teamColor == TeamColor.RED)
                Spawn(redSpawn);
            else
                Spawn(blueSpawn);
        }

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
                foreach (Squad squad in spawnLists[i])
                {
                    for (int n = 0; n < squad.qty; n++)
                    {
                        string unitJson = GodClass.UnitLibrary[squad.combatantType];
                        Combatant c = new BasicMelee(teamColor, unitJson);
                        c.Position = spawnPoint;
                        if(teamColor == TeamColor.RED)
                        {
                            c.PositionX += curCol * widthSpacing;
                        } else
                        {
                            c.PositionX -= curCol * widthSpacing;
                        }
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
