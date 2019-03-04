using Newtonsoft.Json.Linq;
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
        public static Team red;
        public static Team blue;

        public static CardAct GetAction(string actionName, JObject inputParams)
        {
            return functDict[actionName](inputParams);
        }

        private static Dictionary<string, Func<JObject, CardAct>> functDict = new Dictionary<string, Func<JObject, CardAct>>()
        {
            {"addUnit", delegate(JObject json)
            {
                int requiredInputs = 1;
                string combatantType = (string)json["combatantType"];
                int num = (int)json["num"];
                int spawns = (int)json["spawns"];
                Func<int[], int> builtActionFunc = delegate(int[] inputs)
                {
                    TeamColor teamColor = (TeamColor)inputs[0];
                    if(teamColor == TeamColor.RED)
                    {
                        red.combatantSpawner.AddSpawn(num, spawns, combatantType);
                    } else
                    {
                        blue.combatantSpawner.AddSpawn(num, spawns, combatantType);
                    }

                    return 1;
                };
                return new CardAct(builtActionFunc, requiredInputs);
            }}

            ,{"statMultiplier", delegate(JObject json)
            {
                int requiredInputs = 1;
                string statName = (string)json["statName"];
                double statMult = (float)json["statMult"];
                Func<int[], int> builtActionFunc = delegate(int[] inputs)
                {
                    TeamColor teamColor = (TeamColor)inputs[0];
                    if(teamColor == TeamColor.RED)
                    {
                        foreach(Combatant c in red.GetCombatants())
                        {
                            c[statName] = (double)c[statName]*statMult;
                        }
                    } else
                    {
                        foreach(Combatant c in blue.GetCombatants())
                        {
                            c[statName] = (double)c[statName]*statMult;
                        }
                    }

                    return 1;
                };


                return new CardAct(builtActionFunc, requiredInputs);
            }}

            ,{"statSetter", delegate(JObject json)
            {
                int requiredInputs = 1;
                string statName = (string)json["statName"];
                float statVal = (float)json["statVal"];
                Func<int[], int> builtActionFunc = delegate(int[] inputs)
                {
                    TeamColor teamColor = (TeamColor)inputs[0];
                    if(teamColor == TeamColor.RED)
                    {
                        foreach(Combatant c in red.GetCombatants())
                        {
                            c[statName] = statVal;
                        }
                    } else
                    {
                        foreach(Combatant c in blue.GetCombatants())
                        {
                            c[statName] = statVal;
                        }
                    }

                    return 1;
                };


                return new CardAct(builtActionFunc, requiredInputs);
            }}

            ,{"DmgAllUnits", delegate(JObject json)
            {
                int requiredInputs = 1;
                int dmg = (int)json["dmg"];
                Func<int[], int> builtActionFunc = delegate(int[] inputs)
                {
                    TeamColor teamColor = (TeamColor)inputs[0];
                    if(teamColor == TeamColor.RED)
                    {
                        foreach(Combatant c in blue.GetCombatants())
                        {
                            c.UpdateHealth(-dmg);
                        }
                    } else
                    {
                        foreach(Combatant c in red.GetCombatants())
                        {
                            c.UpdateHealth(-dmg);
                        }
                    }

                    return dmg;
                };


                return new CardAct(builtActionFunc, requiredInputs);
            }}


            ,{"HealAllUnits", delegate(JObject json)
            {
                int requiredInputs = 1;
                int healthPts = (int)json["healthPts"];
                Func<int[], int> builtActionFunc = delegate(int[] inputs)
                {
                    TeamColor teamColor = (TeamColor)inputs[0];
                    if(teamColor == TeamColor.RED)
                    {
                        foreach(Combatant c in blue.GetCombatants())
                        {
                            c.UpdateHealth(healthPts);
                        }
                    } else
                    {
                        foreach(Combatant c in red.GetCombatants())
                        {
                            c.UpdateHealth(healthPts);
                        }
                    }

                    return healthPts;
                };


                return new CardAct(builtActionFunc, requiredInputs);
            }}

            ,{"exampleOne", delegate(JObject json)
            {
                int dmg = (int)json["dmg"];
                Func<int> builtActionFunc = delegate()
                {
                    //Console.WriteLine("Dmg is " + dmg);
                    return dmg;
                };
                return new CardAct(builtActionFunc);
            }}

            ,{"exampleTwo", delegate(JObject json)
            {

                int requiredInputs = 1;
                int dmgModifier = (int)json["dmgModifier"];
                Func<int[], int> builtActionFunc = delegate(int[] inputs)
                {

                    int numSacrificedUnits = inputs[0];
                    int totalDmg = numSacrificedUnits*dmgModifier;
                    //Console.WriteLine("Number of Sacrificed Units is " + numSacrificedUnits);
                    //Console.WriteLine("Dmg is " + totalDmg);
                    return totalDmg;
                };


                return new CardAct(builtActionFunc, requiredInputs);
            }}

            ,{"exampleThree", delegate(JObject json)
            {
                int strength = (int)json["strength"];
                Action builtActionFunc = delegate()
                {
                    //Console.WriteLine("Restoring health to MaxHealth x" + (strength*100));
                };
                return new CardAct(builtActionFunc);
            }}
        };
    }
}