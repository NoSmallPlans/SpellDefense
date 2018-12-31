using CocosSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SpellDefense.Common.Entities.Team;

namespace SpellDefense.Common.Entities
{
    public partial class Card : CCNode
    {

        public Card(String cardJson)
        {
            LogicInit(cardJson);
            this.UIInit();
        }
    }
}