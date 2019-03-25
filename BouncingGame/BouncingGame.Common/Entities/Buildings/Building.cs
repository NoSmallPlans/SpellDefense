using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CocosSharp;
using Newtonsoft.Json.Linq;

namespace SpellDefense.Common.Entities.Buildings
{
    public class Building : CCNode
    {
        private int cost;
        private string buildingName;
        private CCSprite buildingSprite;
        private List<Squad> squadsToSpawn;

        public Building(string buildingName)
        {
            this.buildingName = buildingName;
            JObject json = JObject.Parse(GodClass.BuildingDict[this.buildingName]);
            this.squadsToSpawn = new List<Squad>();
            this.cost = (int)json["unitCost"];
            this.buildingSprite = new CCSprite((string)json["buildingSprite"]);
            this.AddChild(buildingSprite);
            JArray startingUnits = (JArray)json["startingUnits"];

            foreach (JObject unit in startingUnits)
            {
                string combatantName = (string)unit["combatantName"];
                int unitCount = (int)unit["unitCount"];
                this.squadsToSpawn.Add(new Squad {qty = unitCount, combatantType = combatantName});
            }
        }

        public List<Squad> GetSquads()
        {
            return this.squadsToSpawn;
        }
        
    }
}
