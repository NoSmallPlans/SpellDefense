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
        int topCard;
        int deckSize;
        TeamColor teamColor;

        public Deck(TeamColor team)
        {
            topCard = 0;
            deckSize = 21;
            teamColor = team;
            cards = new List<Card>();
        }

        public void AddCard(String added)
        {
            cards.Add(new Card(added));
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
