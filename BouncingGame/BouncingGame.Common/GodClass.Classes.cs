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
                    ,'maxMana': '5'
                    ,'baseHealth': '1000'
                    ,'spawnTimer': '15'
                    ,'startingUnits': [
                        {
                            'name': 'soldier'
                            ,'count': '3'
                        },
                        {
                            'name': 'grunt'
                            ,'count': '0'
                        },
                        {
                            'name': 'archer'
                            ,'count': '3'
                        },
                        {
                            'name': 'sniper'
                            ,'count': '0'
                        }
                    ]
                }"
            },
             {
                "ninja",
                @"{
                    'maxHandSize' : '5'
                    ,'maxMana': '5'
                    ,'baseHealth': '1000'
                    ,'spawnTimer': '20'
                    ,'startingUnits': [
                        {
                            'name': 'soldier'
                            ,'count': '3'
                        },
                        {
                            'name': 'grunt'
                            ,'count': '0'
                        },
                        {
                            'name': 'archer'
                            ,'count': '3'
                        },
                        {
                            'name': 'sniper'
                            ,'count': '0'
                        }

                    ]
                }"
            }
        };
    }
}