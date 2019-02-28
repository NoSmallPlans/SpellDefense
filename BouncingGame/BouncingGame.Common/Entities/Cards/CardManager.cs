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
        double cardZoomTime = 0.5f;
        CCLabel manaLabel;
        CCEventListenerTouchAllAtOnce touchListener;
        static List<Action> sharedQueue = new List<Action>();
        static List<Action> redQueue = new List<Action>();
        static List<Action> blueQueue = new List<Action>();
        static int turnCount;


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
        public void UseCard(Card card, CCPoint pos)
        {
            if(currentMana >= card.cardCost)
            {
                UpdateMana(-card.cardCost);
                QueueCard(card, pos);
                card.RemoveFromParent();
                card.State = Card.CardState.Rest;
                hand.Remove(card);
                currentHandSize--;
                DrawCard();
            }
        }

        public void PlayCard(Card card, CCPoint pos)
        {
            card.Play(new int[] { (int)this.teamColor });
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
                        UseCard(card, arg1[0].LocationOnScreen);
                        break;
                    }
                }
            }
        }

        private void QueueCard(Card card, CCPoint pos)
        {
            Card.CardTimeOpts? cardTiming = card.GetCardTiming();
            if(cardTiming == null)
            {
                AddToQueue(() => PlayCard(card, pos));
            }
            else if (cardTiming == Card.CardTimeOpts.Immediate)
            {
                PlayCard(card, pos);
            } else if(cardTiming == Card.CardTimeOpts.Queued)
            {
                AddToQueue(() => PlayCard(card, pos));
            }
            else
            {
                AddToQueue(() => PlayCard(card, pos));
            }
        }

        public static void MergeQueues()
        {
            int i = 0;
            while( i < blueQueue.Count && i < redQueue.Count)
            {
                //on even turns
                if(turnCount % 2 == 0)
                {
                    sharedQueue.Add(blueQueue[i]);
                    sharedQueue.Add(redQueue[i]);
                } else
                {
                    sharedQueue.Add(redQueue[i]);
                    sharedQueue.Add(blueQueue[i]);
                }
                i++;
            }

            if(blueQueue.Count > i)
            {
                while (i < blueQueue.Count)
                {
                    sharedQueue.Add(blueQueue[i]);
                    i++;
                }
                
            }

            if (redQueue.Count > i)
            {
                while (i < redQueue.Count)
                {
                    sharedQueue.Add(redQueue[i]);
                    i++;
                }
                
            }
            redQueue.Clear();
            blueQueue.Clear();
        }

        private void AddToQueue(Action cardPlay)
        {
            if(this.teamColor == TeamColor.BLUE) blueQueue.Add(cardPlay);
            if(this.teamColor == TeamColor.RED) redQueue.Add(cardPlay);
        }

        private static void PlayCardQueue()
        {
            foreach(Action cardPlay in sharedQueue)
            {
                cardPlay();
            }
            sharedQueue.Clear();
        }

        public static void HandleTurnTimeReached(object sender, EventArgs e)
        {
            MergeQueues();
            PlayCardQueue();
            turnCount++;
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
            manaLabel = new CCLabel(manaString, "Arial", 20, CCLabelFormat.SystemFont);
            this.AddChild(manaLabel);
            manaLabel.Color = CCColor3B.White;
            manaLabel.PositionY = 100;
            manaLabel.PositionX = cardStartingX + 40;
        }
    }
}
