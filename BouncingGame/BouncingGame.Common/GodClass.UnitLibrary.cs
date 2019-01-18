﻿using System;
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
                "soldier",
                @"{
                    'attackSpeed' : '5'
                    ,'moveSpeed': '50'
                    ,'attackPwr': '25'
                    ,'spriteImage': 'GreenGuy.png'
                    ,'maxHealth': '100'
                    ,'attackRange': '1'
                    ,'aggroRange': '100'
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
                    ,'spriteImage': 'BlueGuy.png'
                    ,'maxHealth': '100'
                    ,'attackRange': '100'
                    ,'aggroRange': '100'
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
