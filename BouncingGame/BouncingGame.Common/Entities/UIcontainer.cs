using CocosSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpellDefense.Common.Entities
{
    public class UIcontainer : CCNode
    {
        protected CCDrawNode drawNode;
        public CCBoundingBoxI container;
        protected CCLayer targetLayer;
        public float minY;
        public float minX;
        public float maxY;
        public float maxX;
        public float width;
        public float height;
        
        public UIcontainer(int xAnchorPt, int yAnchorPt, int height, int width, CCLayer targetLayer)
        {  
            this.width = width;
            this.height = height;
            this.minX = xAnchorPt;
            this.minY = yAnchorPt;
            this.maxX = xAnchorPt + width;
            this.maxY = yAnchorPt + height;
            this.targetLayer = targetLayer;
            this.container = new CCBoundingBoxI(xAnchorPt, yAnchorPt, (int)this.maxX, (int)this.maxY);
        }

    }
}
