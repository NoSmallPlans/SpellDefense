using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpellDefense.Common
{
    public static partial class GodClass
    {
        public static Dictionary<string, string> ClassConfigs = new Dictionary<string, string>()
        {
             {
                "barbarian",
                @"{
                    'maxHandsize' : '3'
                    ,'maxMana': '5'
                    ,'spawnTimer': '15'
                    ,'baseHealth': '1000'
                    ,'startingUnits': [
                        {
                            'name': 'warrior'
                            ,'count': '3'
                        }
                    ]
                }"
            },
             {
                "ninja",
                @"{
                    'maxHandsize' : '3'
                    ,'maxMana': '5'
                    ,'spawnTimer': '15'
                    ,'baseHealth': '1000'
                    ,'startingUnits': [
                        {
                            'name': 'ninja'
                            ,'count': '3'
                        }
                    ]
                }"
            }
        };
    }
}
