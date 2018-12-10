using CocosSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SpellDefense.Common.Entities.Team;

namespace SpellDefense.Common.Entities
{
    public class Card : CCNode
    {
        public int manaCost;
        CCDrawNode drawNode;
        int height;
        int width;
        CCColor4B background;
        ColorChoice teamColor;
        int index;

        public Action<Card> CardClicked;

        public Card(ColorChoice team, int index)
        {
            height = 75;
            width = 50;
            manaCost = 2;
            this.index = index;
            background = CCColor4B.Red;
            teamColor = team;
        }

        public void Play()
        {
            
        }

        public CCRect GetBoundingBox()
        {
            return new CCRect(this.Position.X, this.Position.Y, this.drawNode.BoundingBox.Size.Width, this.drawNode.BoundingBox.Size.Height);
        }

        public void CreateGraphic()
        {
            drawNode = new CCDrawNode();
            this.AddChild(drawNode);            

            var cardRect = new CCRect(0, 0, width, height);
            drawNode.DrawRect(cardRect, fillColor: background);
            var label = new CCLabel(index.ToString(), "Arial", 20, CCLabelFormat.SystemFont);
            label.Color = CCColor3B.White;
            label.PositionY = 12;
            drawNode.AddChild(label);
        }
    }
}