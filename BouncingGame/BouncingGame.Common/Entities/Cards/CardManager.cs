using CocosSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SpellDefense.Common.Entities.Team;

namespace SpellDefense.Common.Entities.Cards
{
    public class CardManager: CCNode
    {
        Deck deck;
        List<Card> hand;
        int maxHandSize;
        int currentHandSize;
        ColorChoice teamColor;
        int maxMana;
        int currentMana;
        int cardSpacing;
        int cardStartingX;
        CCLabel manaLabel;
        CCEventListenerTouchAllAtOnce touchListener;

        public CardManager(ColorChoice team)
        {
            maxHandSize = 3;
            maxMana = 6;
            currentMana = 6;
            deck = new Deck(team);
            hand = new List<Card>();
            teamColor = team;
            cardSpacing = 100;
            if (team == ColorChoice.BLUE)
                cardStartingX = 400;
            else
                cardStartingX = 0;
            InitDeck();
            InitHand();
            CreateGraphics();
            CreateTouchListener();

            Schedule(t =>
            {
                IncrementMana();
            }, 3);
        }

        public void DrawCard()
        {
            Card card = deck.DrawCard();
            card.CardClicked += CardClicked;
            hand.Add(card);
            currentHandSize++;
            GameCoefficients.cardHUD.AddChild(card);

            UpdateHandPositions();
        }

        private void UpdateHandPositions()
        {
            for(int i = 0; i < currentHandSize; i++)
            {
                hand[i].Position = new CCPoint(i * cardSpacing + cardStartingX, 0);
                hand[i].CreateGraphic();
            }
        }

        private void InitDeck()
        {
            deck.CreateDeck();
        }

        private void InitHand()
        {
            for(int i = 0; i < maxHandSize; i++)
            {
                DrawCard();
            }
        }

        //Check if mana cost is <= player current mana
        //Play Card
        //Remove card from CardHub
        //Draw New Card
        public void CardClicked(Card card)
        {
            if(currentMana >= card.manaCost)
            {
                UpdateMana(-card.manaCost);
                card.Play();
                card.RemoveFromParent();
                card.CardClicked -= CardClicked;
                hand.Remove(card);
                currentHandSize--;
                DrawCard();
            }
        }

        private void CreateTouchListener()
        {
            touchListener = new CCEventListenerTouchAllAtOnce();
            touchListener.OnTouchesEnded = HandleTouchesEnded;
            AddEventListener(touchListener, this);
        }

        private void HandleTouchesEnded(List<CCTouch> arg1, CCEvent arg2)
        {
            foreach (Card c in hand)
            {
                if (c.GetBoundingBox().ContainsPoint(arg1[0].Location))
                {
                    CardClicked(c);
                    break;
                }
            }
        }

        private void UpdateMana(int manaDelta)
        {
            currentMana += manaDelta;
            manaLabel.Text = "Mana:" + currentMana.ToString() + "/" + maxMana.ToString();
        }

        private void IncrementMana()
        {
            if (currentMana < maxMana)
                UpdateMana(1);
        }

        private void CreateGraphics()
        {
            string manaString = "Mana:" + currentMana.ToString() + "/" + maxMana.ToString();
            manaLabel = new CCLabel(manaString, "Arial", 20, CCLabelFormat.SystemFont);
            this.AddChild(manaLabel);
            manaLabel.Color = CCColor3B.White;
            manaLabel.PositionY = 100;
            manaLabel.PositionX = cardStartingX + 40;
        }
    }
}
