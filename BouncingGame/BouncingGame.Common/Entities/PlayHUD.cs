using CocosSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpellDefense.Common.Entities
{
    class PlayHUD : UIcontainer
    {
        public PlayHUD(int yAnchorPt, int xAnchorPt, int height, int width, CCLayer Layer) : base (yAnchorPt, xAnchorPt, height, width, Layer)
        {
            this.drawBackground();
        }

        public void drawBackground()
        {
            this.drawNode = new CCDrawNode();
            this.AddChild(drawNode);
            this.drawNode.DrawRect(new CCRect(this.minX, this.minY, (float)this.width, (float)this.height),
                fillColor: CCColor4B.Green,
                borderWidth: 0,
                borderColor: CCColor4B.Green);
            this.Layer.AddChild(drawNode);
        }
    }
}
