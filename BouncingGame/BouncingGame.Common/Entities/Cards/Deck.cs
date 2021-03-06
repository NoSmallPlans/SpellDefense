﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SpellDefense.Common.Entities.Team;
using static SpellDefense.Common.GodClass;

namespace SpellDefense.Common.Entities.Cards
{
    public class Deck
    {
        List<Card> cards;
        int cardsLeft;
        int deckSize;
        Random rnd;
        Common.GodClass.TeamColor teamColor;

        public Deck(Common.GodClass.TeamColor team)
        {
            cardsLeft = 6;
            deckSize = 6;
            teamColor = team;
            cards = new List<Card>();
            rnd = new Random();
        }

        public void InitFromJson(string json)
        {
            cards = new List<Card>();
            deckSize = 0;
            JObject testJson = JObject.Parse(json);
            JArray cardJArray = (JArray)testJson["cards"];

            foreach (JObject card in cardJArray)
            {
                string cardName = (string)card["name"];
                int count = (int)card["count"];
                for(int i = 0; i < count; i++)
                {
                    cards.Add(new Card(GodClass.CardLibrary[cardName]));
                    deckSize++;
                }
            }
            cardsLeft = deckSize;
        }

        public void AddCard(String added)
        {
            cards.Add(new Card(added));
        }

        public Card DrawCard()
        {
            if (cardsLeft <= 0)
            {
                cardsLeft = deckSize;
            }
            //Pick a random index between 0 and cards left
            //We want to move this card to the end of the list
            //Then return it
            int index = rnd.Next(0, cardsLeft);
            cardsLeft--;
            cards.Add(cards[index]);
            cards.RemoveAt(index);
            return cards[deckSize - 1];
        }
    }
}
