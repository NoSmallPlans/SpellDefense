using CocosSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SpellDefense.Common.Entities.Team;
using static SpellDefense.Common.GodClass;
using static SpellDefense.Common.Networking.Messaging;

namespace SpellDefense.Common.Entities.Cards
{
    public class CardManager: CCNode
    {
        Deck deck;
        List<Card> hand;
        int maxHandSize;
        int currentHandSize;
        TeamColor teamColor;
        int maxMana;
        int currentMana;
        int cardSpacing;
        int cardStartingX;
        int cardStartingY;
        DateTime timeCardTouched;
        float cardZoomTime = 0.5f;
        CCLabel manaLabel;
        CCEventListenerTouchAllAtOnce touchListener;
        

        public CardManager(TeamColor team)
        {
            maxHandSize = 3;
            maxMana = 6;
            currentMana = 6;
            deck = new Deck(team);
            hand = new List<Card>();
            teamColor = team;
            cardSpacing = 100;
            if (team == TeamColor.BLUE)
                cardStartingX = 700;
            else
                cardStartingX = 0;
            cardStartingY = -100;
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
            hand.Add(card);
            currentHandSize++;
            GodClass.cardHUD.AddChild(card);

            UpdateHandPositions();
        }

        private void UpdateHandPositions()
        {
            for(int i = 0; i < currentHandSize; i++)
            {
                hand[i].Position = new CCPoint(i * cardSpacing + cardStartingX, cardStartingY);
                hand[i].OriginalPosition = hand[i].Position;
            }
        }

        private void InitDeck()
        {
            if(teamColor == TeamColor.RED)
            {
                deck.InitFromJson(GodClass.Decks[GodClass.playerOneDeck]);
            }
            else
            {
                deck.InitFromJson(GodClass.Decks[GodClass.playerTwoDeck]);
            }
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
        public void PlayCard(Card card, CCPoint pos)
        {
            if(currentMana >= card.cardCost)
            {
                UpdateMana(-card.cardCost);
                if (GodClass.online)
                {
                    //Send Message to server
                    GodClass.clientRef.AddOutMessage(ConstructCardMessage(false, card.CardName.ToLower()));
                }
                else
                {
                    //Play card locally
                    GodClass.clientRef.ParseMessage(ConstructCardMessage(true, card.CardName.ToLower()));
                }
                card.RemoveFromParent();
                card.State = Card.CardState.Rest;
                hand.Remove(card);
                GodClass.cardHistory.AddToHistory(card, teamColor);
                currentHandSize--;
                DrawCard();
            }
        }

        private string ConstructCardMessage(bool self, string cardName)
        {
            string message = cardName + ";" + ((int)teamColor).ToString();
            string msgType;
            msgType = ((int)MsgType.PlayCard).ToString();
            return msgType + "," + message;
        }

        private void CreateTouchListener()
        {
            touchListener = new CCEventListenerTouchAllAtOnce();
            touchListener.OnTouchesEnded = HandleTouchesEnded;
            touchListener.OnTouchesMoved = HandleTouchesMoved;
            touchListener.OnTouchesBegan = HandleTouchesBegan;
            AddEventListener(touchListener, this);
        }

        private void HandleTouchesMoved(List<CCTouch> arg1, CCEvent arg2)
        {
            foreach (Card c in hand)
            {
                if (c.GetBoundingBox().ContainsPoint(arg1[0].Location))
                {
                    DateTime checkTime = DateTime.Now;
                    if ((checkTime - timeCardTouched).TotalSeconds >= cardZoomTime)
                    {
                        c.State = Card.CardState.Expanded;                        
                    }
                    break;
                }
            }
        }

        private void HandleTouchesBegan(List<CCTouch> arg1, CCEvent arg2)
        {
            foreach (Card c in hand)
            {
                if (c.GetBoundingBox().ContainsPoint(arg1[0].Location))
                {
                    timeCardTouched = DateTime.Now;
                    break;
                }
            }
        }

        private void HandleTouchesEnded(List<CCTouch> arg1, CCEvent arg2)
        {
            foreach (Card c in hand)
            {
                if (c.GetBoundingBox().ContainsPoint(arg1[0].Location))
                {
                    if(c.State == Card.CardState.Rest)
                    {
                        DeselectCards();
                        c.State = Card.CardState.Selected;
                    }
                    else
                    {
                        c.State = Card.CardState.Rest;
                    }
                    return;
                }
                if(c.State == Card.CardState.Expanded)
                {
                    c.State = Card.CardState.Rest;
                }
            }
            if (GodClass.BattlefieldDimensions.GetBounds().ContainsPoint(arg1[0].Location))
            {
                foreach (Card card in hand)
                {
                    if (card.State == Card.CardState.Selected)
                    {
                        PlayCard(card, arg1[0].LocationOnScreen);
                        break;
                    }
                }
            }
        }

        private void DeselectCards()
        {
            foreach(Card c in hand)
            {
                c.State = Card.CardState.Rest;
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
            manaLabel = new CCLabel(manaString, "Arial", 24, CCLabelFormat.SystemFont);
            this.AddChild(manaLabel);
            manaLabel.Color = CCColor3B.White;
            manaLabel.PositionY = 130;
            manaLabel.PositionX = cardStartingX + 50;
        }
    }
}
