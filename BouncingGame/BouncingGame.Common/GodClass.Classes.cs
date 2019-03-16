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
        public static string playerTwoClass = "barbarian";
        public static Dictionary<string, string> ClassConfigs = new Dictionary<string, string>()
        {
             {
                "barbarian",
                @"{
                    'maxHandSize' : '5'
                    ,'maxMana': '5'
                    ,'baseHealth': '1000'
                    ,'spawnTimer': '10'
                    ,'startingUnits': [
                        {
                            'name': 'soldier'
                            ,'count': '1'
                        },
                        {
                            'name': 'grunt'
                            ,'count': '0'
                        },
                        {
                            'name': 'archer'
                            ,'count': '1'
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