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
        CCDrawNode drawNode;
        int height;
        int width;
        CCColor4B background;
        public Action<Card> CardClicked;

        private void UIInit()
        {
            height = 75;
            width = 50;
            background = CCColor4B.Red;
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
            var label = new CCLabel(cardTitle.ToString(), "Arial", 20, CCLabelFormat.SystemFont);
            label.Color = CCColor3B.White;
            label.PositionY = 12;
            drawNode.AddChild(label);
        }
    }
}