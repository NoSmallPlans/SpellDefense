using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SpellDefense.Common.Entities.Team;

namespace SpellDefense.Common.Entities.Cards
{
    public class Deck
    {
        List<Card> cards;
        int topCard;
        int deckSize;
        ColorChoice teamColor;

        public Deck(ColorChoice team)
        {
            topCard = 0;
            deckSize = 21;
            teamColor = team;
            cards = new List<Card>();
        }

        //Creates a random deck of cards
        public void CreateDeck()
        {
            for(int i = 0; i < deckSize; i++)
            {
                cards.Add(new Card(teamColor, i));
            }
        }

        public void Shuffle()
        {
            topCard = 0;
            //TODO Add shuffle logic to mix cards in the deck
        }

        public Card DrawCard()
        {
            if(topCard == cards.Count)
            {
                Shuffle();
            }
            return cards[topCard++];
        }
    }
}
