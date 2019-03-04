using SpellDefense.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpellDefense.Common
{
    public static partial class GodClass
    {
        public static Dictionary<string, Card> library;

        public static void InitLibrary()
        {
            Card c;
            library = new Dictionary<string, Card>();
            foreach (string key in CardLibrary.Keys)
            {
                c = new Card(CardLibrary[key]);
                library.Add(key, c);
            }
        }

        public static void PlayCard(string name, int[] args)
        {
            library[name].Play(args);
        }

        public static Dictionary<string, string>  CardLibrary = new Dictionary<string, string>()
        {
            {
                "slow",
                @"{
                    'cardTitle' : 'Slow'
                    ,'cardText': 'Halves unit speed'
                    ,'cardCost': '0'
                    ,'cardImage': 'GreenGuy.png'
                    ,'cardActions': [
                        {
                            'actionName': 'statMultiplier'
                            ,'compileTimeArgs': {
                                'statName' : 'moveSpeed'
                                ,'statMult' : '0.5'
                            }
                        }
                    ]
                }"
            },
            {
                "spawn ranged",
                @"{
                    'cardTitle' : 'Spawn Ranged'
                    ,'cardText': 'Adds archer to the next spawn'
                    ,'cardCost': '2'
                    ,'cardImage': 'BlueGuy.png'
                    ,'cardActions': [
                        {
                            'actionName': 'addUnit'
                            ,'compileTimeArgs': {
                                'combatantType' : 'archer'
                                ,'num' : '1'
                                ,'spawns' : '1'
                            }
                        }
                    ]
                }"
            },
            {
                "spawn soldier",
                @"{
                    'cardTitle' : 'Spawn Soldier'
                    ,'cardText': 'Adds soldier to the next spawn'
                    ,'cardCost': '1'
                    ,'cardImage': 'BlueGuy.png'
                    ,'cardActions': [
                        {
                            'actionName': 'addUnit'
                            ,'compileTimeArgs': {
                                'combatantType' : 'soldier'
                                ,'num' : '1'
                                ,'spawns' : '1'
                            }
                        }
                    ]
                }"
            },
            {
                "haste",
                @"{
                    'cardTitle' : 'Haste'
                    ,'cardText': 'Give all my units 50 speed'
                    ,'cardCost': '0'
                    ,'cardImage': 'GreenGuy.png'
                    ,'cardActions': [
                        {
                            'actionName': 'statSetter'
                            ,'compileTimeArgs': {
                                'statName' : 'moveSpeed'
                                ,'statVal' : '50.0'
                            }
                        }
                    ]
                }"
            },
            {
                "nuke",
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
            },
            {
                "fireball",
                @"{
                    'cardTitle' : 'Fireball'
                    ,'cardText': 'Deal 50 damage to all bad guys' 
                    ,'cardCost': '2'
                    ,'cardImage': 'fireball.png'
                    ,'cardActions': [
                        {
                            'actionName': 'DmgAllUnits'
                            ,'compileTimeArgs': {'dmg' : '50'}
                        }
                    ]
                }"
            },
            {
                "spawn grunt",
                @"{
                    'cardTitle' : 'Spawn Grunt'
                    ,'cardText': 'Adds grunt to the next spawn'
                    ,'cardCost': '3'
                    ,'cardImage': 'GreenGuy.png'
                    ,'cardActions': [
                        {
                            'actionName': 'addUnit'
                            ,'compileTimeArgs': {
                                'combatantType' : 'grunt'
                                ,'num' : '1'
                                ,'spawns' : '1'
                            }
                        }
                    ]
                }"
            },
            {
                "spawn sniper",
                @"{
                    'cardTitle' : 'Spawn Sniper'
                    ,'cardText': 'Adds grunt to the next spawn'
                    ,'cardCost': '4'
                    ,'cardImage': 'BlueGuy.png'
                    ,'cardActions': [
                        {
                            'actionName': 'addUnit'
                            ,'compileTimeArgs': {
                                'combatantType' : 'sniper'
                                ,'num' : '1'
                                ,'spawns' : '1'
                            }
                        }
                    ]
                }"
            },
        };
    }
}
