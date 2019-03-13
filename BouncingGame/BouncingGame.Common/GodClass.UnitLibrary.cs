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
                "skeleton",
                @"{
                    'attackSpeed' : '1'
                    ,'moveSpeed': '25'
                    ,'attackPwr': '25'
                    ,'maxHealth': '100'
                    ,'attackRange': '1'
                    ,'aggroRange': '100'
                    ,'sprite': 'skeleton'
                }"
            },
             {
                "earthwisp",
                @"{
                    'attackSpeed' : '1'
                    ,'moveSpeed': '100'
                    ,'attackPwr': '25'
                    ,'maxHealth': '100'
                    ,'attackRange': '600'
                    ,'aggroRange': '100'
                    ,'shootsProjectile': 'true'
                    ,'projSprite': 'earthProjectile'
                    ,'projSpeed': '150'
                    ,'sprite': 'earthwisp'
                }"
            },
             {
                "firewisp",
                @"{
                    'attackSpeed' : '1'
                    ,'moveSpeed': '100'
                    ,'attackPwr': '25'
                    ,'maxHealth': '100'
                    ,'attackRange': '600'
                    ,'aggroRange': '100'
                    ,'shootsProjectile': 'true'
                    ,'projSprite': 'fireProjectile'
                    ,'projSpeed': '150'
                    ,'sprite': 'firewisp'
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
                }"
            },
             {
                "soldier",
                @"{
                    'attackSpeed' : '1'
                    ,'moveSpeed': '25'
                    ,'attackPwr': '25'
                    ,'maxHealth': '100'
                    ,'attackRange': '1'
                    ,'aggroRange': '100'
                    ,'fillColor': '0,255,255'
                    ,'abilities': [
                        {
                            'actionName': 'statMultiplier'
                            ,'compileTimeArgs': {
                                'statName' : 'speed'
                                ,'statMult' : '0.5'
                            }
                        }
                    ]
                }"
            },
             {
                "archer",
                @"{
                    'attackSpeed' : '5'
                    ,'moveSpeed': '35'
                    ,'attackPwr': '25'
                    ,'maxHealth': '100'
                    ,'attackRange': '100'
                    ,'aggroRange': '100'
                    ,'fillColor': '0,255,0'
                    ,'abilities': [
                        {
                            'actionName': 'statMultiplier'
                            ,'compileTimeArgs': {
                                'statName' : 'speed'
                                ,'statMult' : '0.5'
                            }
                        }
                    ]
                }"
            }
        };
    }
}
