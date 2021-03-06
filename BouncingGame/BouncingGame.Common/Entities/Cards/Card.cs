﻿using CocosSharp;
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
        public enum CardState
        {
            Rest,
            Selected,
            Expanded
        };

        private CardState state;

        public enum CardTimeOpts
        {
            Immediate
            , Queued
        };

        private CardTimeOpts? cardTiming;

        public CardTimeOpts? GetCardTiming(){
            return this.cardTiming;
        }

        public Card(String cardJson)
        {
            LogicInit(cardJson);
            this.UIInit();
            state = CardState.Rest;
        }
    }
}