using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CocosSharp;
using Newtonsoft.Json.Linq;

namespace SpellDefense.Common.Entities.Buildings
{
    public class BuildingMgr
    {
        int currentMoney;
        int maxMoney;
        int moneyAccumRate;
        List<Building> ownedBuildings;
        CCMenu buildingMenu;

        public BuildingMgr()
        {
            List<CCMenuItemImage> buildingMenuOpts = new List<CCMenuItemImage>();
            this.ownedBuildings = new List<Building>();
            CCMenuItem[] buildingMenuItems = new CCMenuItem[GodClass.BuildingDict.Count];
            int i = 0;
            foreach (KeyValuePair<string, string> entry in GodClass.BuildingDict)
            {
                JObject buildingJson = JObject.Parse(entry.Value);
                string selectableMenuSprite = (string)buildingJson["selectableMenuSprite"];
                string disabledMenuSprite = (string)buildingJson["disabledMenuSprite"];
                buildingMenuItems[i] = new CCMenuItemImage(selectableMenuSprite, disabledMenuSprite, GetBuildingAdder(entry.Key));

                i++;
            }
            this.buildingMenu = new CCMenu(buildingMenuItems);
            this.buildingMenu.Position = new CCPoint(600, 350);
            this.buildingMenu.AlignItemsHorizontally(10);
            GodClass.hudLayer.AddChild(buildingMenu);
        }

        public Action<object> GetBuildingAdder(string buildingName)
        {
            Action<object> buildingAdderAction = delegate (object sender)
            {
                Building addedBuilding = new Building(buildingName);
                ownedBuildings.Add(addedBuilding);
                GodClass.gridManager.PlaceTile(addedBuilding, new CCPoint(5, 5));
            };
            return buildingAdderAction;
        }
    }
}
