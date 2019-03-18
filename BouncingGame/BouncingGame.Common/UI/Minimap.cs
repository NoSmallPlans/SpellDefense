using CocosSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpellDefense.Common.UI
{
    public class Minimap : CCNode
    {
        CCDrawNode drawNode;
        CCRect backgroundRect;
        CCRect curLocRect;
        CCNode layerToScroll;
        CCEventListenerTouchAllAtOnce touchListener;
        float redBarHeight, redBarWidth;

        public Minimap(CCNode scrollLayer, float width, float height)
        {
            layerToScroll = scrollLayer;
            CreateDrawNode(width, height);
            redBarHeight = height;
            redBarWidth = 40;
            CreateTouchListener();
        }

        private void CreateDrawNode(float width, float height)
        {
            drawNode = new CCDrawNode();
            this.AddChild(drawNode);
            backgroundRect = new CCRect(0, 0, width, height);
            DrawGraphic(CCPoint.Zero);
            this.ContentSize = drawNode.ScaledContentSize;
        }

        private void DrawGraphic(CCPoint loc)
        {
            drawNode.Clear();
            drawNode.DrawRect(backgroundRect, fillColor: CCColor4B.Yellow);
            curLocRect = new CCRect(loc.X, 0, redBarWidth, redBarHeight);
            drawNode.DrawRect(curLocRect, fillColor: CCColor4B.Red);
        }

        private void CreateTouchListener()
        {
            touchListener = new CCEventListenerTouchAllAtOnce();
            //touchListener.OnTouchesEnded = HandleTouchesEnded;
            touchListener.OnTouchesMoved = HandleTouchesMoved;
            touchListener.OnTouchesBegan = HandleTouchesBegan;
            AddEventListener(touchListener, this);
        }

        private void HandleTouchesBegan(List<CCTouch> arg1, CCEvent arg2)
        {
            if (this.BoundingBoxTransformedToWorld.ContainsPoint(arg1[0].Location))
            {
                UpdateScroll(arg1[0].Location);
            }
        }

        private void HandleTouchesMoved(List<CCTouch> arg1, CCEvent arg2)
        {
            if (this.BoundingBoxTransformedToWorld.ContainsPoint(arg1[0].Location))
            {
                UpdateScroll(arg1[0].Location);
            }
        }

        private void UpdateScroll(CCPoint touchPoint)
        {
            CCPoint correctedLoc = new CCPoint(touchPoint.X - this.Position.X, touchPoint.Y);
            DrawGraphic(correctedLoc);

            float bfWidth = GodClass.BattlefieldDimensions.GetWidth();
            float percentage = correctedLoc.X / this.ScaledContentSize.Width;
            float oneScreenWidth = GodClass.desiredWidth;
            layerToScroll.PositionX = (-bfWidth + oneScreenWidth ) * percentage;
            if (Math.Abs(layerToScroll.PositionX) > bfWidth)
                layerToScroll.PositionX = -bfWidth;
        }
    }
}
