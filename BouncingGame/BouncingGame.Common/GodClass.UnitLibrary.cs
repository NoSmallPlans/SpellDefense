using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpellDefense.Common
{
    public static partial class GodClass
    {
        public static Dictionary<string, string> UnitLibrary = new Dictionary<string, string>()
        {
             {
                "rock",
                @"{
                    'attackSpeed' : '1'
                    ,'moveSpeed': '50'
                    ,'attackPwr': '25'
                    ,'armor': '0'
                    ,'sprite': 'Rock_1.png'
                    ,'maxHealth': '100'
                    ,'attackRange': '1'
                    ,'aggroRange': '500'
                    ,'fillColor': '0,255,255'
                }"
            },
             {
                "archer",
                @"{
                    'attackSpeed' : '5'
                    ,'moveSpeed': '50'
                    ,'attackPwr': '25'
                    ,'sprite': 'BlueGuy.png'
                    ,'maxHealth': '100'
                    ,'attackRange': '100'
                    ,'aggroRange': '500'
                    ,'fillColor': '0,255,0'
                }"
            },
            {
                "minotaur",
                @"{
                    'attackSpeed' : '4'
                    ,'moveSpeed': '120'
                    ,'attackPwr': '50'
                    ,'maxHealth': '100'
                    ,'attackRange': '200'
                    ,'aggroRange': '100'
                    ,'sprite': 'minotaur'
                    ,'animations': [
                        {
                            'name' : 'move'
                            ,'delay' : '0.1'
                            ,'repeat' : 'true'
                        }
                        ,{
                            'name' : 'attack'
                            ,'delay' : '0.1'
                            ,'repeat' : 'false'
                        }
                        ,{
                            'name' : 'idle'
                            ,'delay' : '0.1'
                            ,'repeat' : 'true'
                        }
                        ,{
                            'name' : 'die'
                            ,'delay' : '0.1'
                            ,'repeat' : 'false'
                        }
                    ]
                }"
            },
            {
                "grunt",
                @"{
                    'attackSpeed' : '1'
                    ,'moveSpeed': '50'
                    ,'attackPwr': '40'
                    ,'armor': '2'
                    ,'sprite': 'GreenGuy.png'
                    ,'maxHealth': '175'
                    ,'attackRange': '1'
                    ,'aggroRange': '500'
                    ,'fillColor': '255,255,0'
                }"
            },
            {
                "sniper",
                @"{
                    'attackSpeed' : '50'
                    ,'moveSpeed': '8'
                    ,'attackPwr': '100'
                    ,'armor': '0'
                    ,'sprite': 'BlueGuy.png'
                    ,'maxHealth': '100'
                    ,'attackRange': '600'
                    ,'aggroRange': '500'
                    ,'fillColor': '0,0,255'
                }"
            },
        };
    }
}
