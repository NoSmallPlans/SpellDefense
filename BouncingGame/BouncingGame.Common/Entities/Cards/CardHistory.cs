using CocosSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpellDefense.Common.Entities.Cards
{
    public class CardHistory : UIcontainer
    {
        List<CCSprite> cardIconsPlayed;
        //todo center display
        //todo scale cards

        private static float SCALED_HEIGHT = 0.2f;
        private static float SCALED_WIDTH = 0.75f;
        private static int CARD_WIDTH = 100;
        private static int CARD_BUFFER = 10;
        private static int HISTORY_LIMIT = 5;
        private int yAnchor = 0;
        private int xAnchor = 0;

        public CardHistory(int xAnchorPt, int yAnchorPt, int height, int width, CCLayer targetLayer) : base(xAnchorPt, yAnchorPt, ScaleHeight(height), ScaleWidth(width), targetLayer)
        {
            this.yAnchor = yAnchorPt;
            this.xAnchor = xAnchorPt;
            this.cardIconsPlayed = new List<CCSprite>();
            //this.drawBackground();
        }

        public void AddToHistory(Card card, GodClass.TeamColor teamColor)
        {
            CCSprite cardIcon = card.CloneCardIcon();
            CCColor4B teamBorderColor = teamColor == GodClass.TeamColor.BLUE ? CCColor4B.Blue : CCColor4B.Red;

            CCDrawNode drawNode = new CCDrawNode();
            drawNode.DrawRect(new CCRect(0, 0, 17, 17),
                fillColor: CCColor4B.Transparent,
                borderWidth: 0.5f,
                borderColor: teamBorderColor);
            drawNode.ZOrder = this.ZOrder - 1;
            cardIcon.AddChild(drawNode);
            cardIcon.AnchorPoint = new CCPoint(0,0);
            this.AddChild(cardIcon);
            this.cardIconsPlayed.Add(cardIcon);
            this.MaintainMaxHistorySize();
            cardIcon.PositionY = yAnchor;
            for (int i = 0; i < cardIconsPlayed.Count; i++)
            {
                cardIconsPlayed[i].PositionX = xAnchor + CardBuffer(i);
            }
            

        }

        private static int ScaleHeight(int height)
        {
            
            return (int)(SCALED_HEIGHT * height);
        }

        private static int ScaleWidth(int width)
        {
            
            return (int)(SCALED_WIDTH * width);
        }


        public void drawBackground()
        {
            this.drawNode = new CCDrawNode();
            this.AddChild(drawNode);
            this.drawNode.DrawRect(new CCRect(this.minX, this.minY, (float)this.width, (float)this.height),
                fillColor: CCColor4B.Green,
                borderWidth: 0,
                borderColor: CCColor4B.Green);
            this.targetLayer.AddChild(drawNode);
        }

        private int CardBuffer(int i)
        {
            int offsetCount = i - 1; //subtract one, bc no need to offset for first card
            return (CARD_BUFFER + CARD_WIDTH) * offsetCount;
        }

        private void MaintainMaxHistorySize()
        {

            if(this.cardIconsPlayed.Count > HISTORY_LIMIT)
            {
                this.cardIconsPlayed[0].RemoveFromParent();
                this.cardIconsPlayed.RemoveAt(0);   
            }
        }
    }
}
