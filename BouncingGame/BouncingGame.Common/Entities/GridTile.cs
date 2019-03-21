using CocosSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpellDefense.Common.Entities
{
    public class GridTile : CCNode
    {
        public bool filled;
        CCSprite gridSprite;
        int xLoc, yLoc;

        public GridTile(string spriteImage, int x, int y)
        {
            gridSprite = new CCSprite(spriteImage);
            this.AddChild(gridSprite);
            xLoc = x;
            yLoc = y;
        }
    }
}
