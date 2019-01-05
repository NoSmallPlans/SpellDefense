using CocosSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpellDefense.Common.Entities
{
    class CardHUD : UIcontainer
    {
        public CardHUD(int yAnchorPt, int xAnchorPt, int height, int width, CCLayer targetLayer) : base (yAnchorPt, xAnchorPt, height, width, targetLayer)
        {
            this.drawBackground();
        }

        public void drawBackground()
        {
            this.drawNode = new CCDrawNode();
            this.AddChild(drawNode);
            this.drawNode.DrawRect(new CCRect(this.minX, this.minY, (float)this.width, (float)this.height),
                fillColor: CCColor4B.Gray,
                borderWidth: 0,
                borderColor: CCColor4B.Gray);
            this.targetLayer.AddChild(drawNode);
        }
    }
}
