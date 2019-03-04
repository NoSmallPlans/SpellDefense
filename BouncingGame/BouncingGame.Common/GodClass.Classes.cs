using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpellDefense.Common
{
    public static partial class GodClass
    {
        public static string playerOneClass = "barbarian";
        public static string playerTwoClass = "ninja";
        public static Dictionary<string, string> ClassConfigs = new Dictionary<string, string>()
        {
             {
                "barbarian",
                @"{
                    'maxHandSize' : '5'
                    ,'maxMana': '10'
                    ,'spawnTimer': '15'
                    ,'baseHealth': '1000'
                    ,'startingUnits': [
                        {
                            'name': 'soldier'
                            ,'count': '3'
                        }
                    ]
                }"
            },
             {
                "ninja",
                @"{
                    'maxHandSize' : '3'
                    ,'maxMana': '5'
                    ,'spawnTimer': '15'
                    ,'baseHealth': '1000'
                    ,'startingUnits': [
                        {
                            'name': 'archer'
                            ,'count': '3'
                        }
                    ]
                }"
            }
        };
    }
}