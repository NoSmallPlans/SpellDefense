using CocosSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SpellDefense.Common.Entities.Team;
using Newtonsoft.Json.Linq;

namespace SpellDefense.Common.Entities
{
    public partial class Card : CCNode
    {
        private List<CardAct> cardActionList = new List<CardAct>();
        string cardTitle;
        string cardText;
        string cardImage;
        public int cardCost;
        public void LogicInit(String text)
        {
            JObject testJson = JObject.Parse(text);
            cardTitle = (string)testJson["cardTitle"];
            cardText = (string)testJson["cardText"];
            cardImage = (string)testJson["cardImage"];
            cardCost = (int)testJson["cardCost"];
            JArray cardActions = (JArray)testJson["cardActions"];

            foreach (JObject cardAction in cardActions)
            {
                string cardName = (string)cardAction["actionName"];
                JObject compileTimeArgs = (JObject)cardAction["compileTimeArgs"];
                CardAct tempAction = GodClass.GetAction(cardName, compileTimeArgs);
                this.cardActionList.Add(tempAction);
            }
        }

        public void Play(int[] inputArgs = null)
        {
            int indxPtr = 0;
            int[] actionSpecificArgs;
            int actionArgCount;
            foreach (CardAct act in cardActionList)
            {
                if (act.GetNumReqArgs() > 0)
                {
                    actionArgCount = act.GetNumReqArgs();
                    actionSpecificArgs = new int[actionArgCount];
                    for (int i = 0; i < actionArgCount; i++)
                    {
                        actionSpecificArgs[i] = inputArgs[indxPtr];
                        indxPtr++;
                    }
                    act.perform(actionSpecificArgs);
                }
                else
                {
                    act.perform();
                }
            }
        }
    }
}
