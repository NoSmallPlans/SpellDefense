using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpellDefense.Common
{
    public static partial class GodClass
    {
        public static Dictionary<string, string> BuildingDict = new Dictionary<string, string>()
        {
             {
                "barracks",
                @"{
                    'unitCost' : '5'
                    ,'buildingSprite': 'command_center.png'
                    ,'selectableMenuSprite': 'command_center.png'
                    ,'disabledMenuSprite': 'command_center.png'
                    ,'startingUnits': [
                        {
                            'combatantName': 'minotaur'
                            ,'unitCount': '2'
                        },
                        {
                            'combatantName': 'archer'
                            ,'unitCount': '1'
                        }
                    ]
                }"
            }
             ,{
                "archeryRange",
                @"{
                    'unitCost' : '5'
                    ,'buildingSprite': 'command_center.png'
                    ,'selectableMenuSprite': 'command_center.png'
                    ,'disabledMenuSprite': 'command_center.png'
                    ,'startingUnits': [
                        {
                            'combatantName': 'archer'
                            ,'unitCount': '3'
                        }
                    ]
                }"
            }
        };
    }
}