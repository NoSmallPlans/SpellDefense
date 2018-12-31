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
                'cardTitle' : 'Damage Bad Guys'
                ,'cardText': 'Deal 75 damage to all bad guys'
                ,'cardCost': '5'
                ,'cardActions': [
                    {
                        'actionName': 'DmgAllUnits'
                        ,'compileTimeArgs': {'dmg' : '75'}
                    }
                ]
            }"
            ,@"{
                'cardTitle' : 'Tiny Damage Bad Guys'
                ,'cardText': 'Deal 10 damage to all bad guys' 
                ,'cardCost': '2'
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
