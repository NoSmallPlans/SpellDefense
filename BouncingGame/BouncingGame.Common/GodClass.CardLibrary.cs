using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpellDefense.Common
{
    public static partial class GodClass
    {
        public static String[] CardLibrary =
        {
            @"{
                'cardTitle' : 'Haste'
                ,'cardText': 'Give all my units 700 speed'
                ,'cardCost': '0'
                ,'cardImage': 'GreenGuy.png'
                ,'cardActions': [
                    {
                        'actionName': 'statSetter'
                        ,'compileTimeArgs': {
                            'statName' : 'speed'
                            ,'statVal' : '700.0'
                        }
                    }
                ]
            }"
            ,
            @"{
                'cardTitle' : 'Nuke'
                ,'cardText': 'Deal 75 damage to all bad guys'
                ,'cardCost': '5'
                ,'cardImage': 'nuke.png'
                ,'cardActions': [
                    {
                        'actionName': 'DmgAllUnits'
                        ,'compileTimeArgs': {'dmg' : '75'}
                    }
                ]
            }"
            ,@"{
                'cardTitle' : 'Fireball'
                ,'cardText': 'Deal 10 damage to all bad guys' 
                ,'cardCost': '2'
                ,'cardImage': 'fireball.png'
                ,'cardActions': [
                    {
                        'actionName': 'DmgAllUnits'
                        ,'compileTimeArgs': {'dmg' : '10'}
                    }
                ]
            }"
        };
    }
}
