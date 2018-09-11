using CocosSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpellDefense.Common.Entities
{
    class UIcontainer : CCNode
    {
        protected CCDrawNode drawNode;
        public CCBoundingBoxI container;
        protected CCLayer Layer;
        protected float minY;
        protected float minX;
        protected float maxY;
        protected float maxX;
        protected float width;
        protected float height;
        
        public UIcontainer(int xAnchorPt, int yAnchorPt, int height, int width, CCLayer Layer)
        {  
            this.width = width;
            this.height = height;
            this.minX = xAnchorPt;
            this.minY = yAnchorPt;
            this.maxX = xAnchorPt + width;
            this.maxY = yAnchorPt + height;
            this.Layer = Layer;
            this.container = new CCBoundingBoxI(xAnchorPt, yAnchorPt, (int)this.maxX, (int)this.maxY);
        }

    }
}
