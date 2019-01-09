using CocosSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpellDefense.Common.UI
{
    public class Button : CCNode
    {
        CCDrawNode drawNode;
        CCLabel label;
        CCColor4B backColor;
        CCColor4B borderColor;
        float borderWidth;
        float width;
        float height;
        string text;
        string fontType;
        int fontSize;
        CCColor3B fontColor;

        public Button()
        {
        }

        public Button(CCColor4B backColor, CCColor4B borderColor, float width, float height, float borderWidth, string text, CCColor3B fontColor, string fontType="Arial", int fontSize=16)
        {
            this.backColor = backColor;
            this.borderColor = borderColor;
            this.width = width;
            this.height = height;
            this.borderWidth = borderWidth;
            this.text = text;
            this.fontType = fontType;
            this.fontSize = fontSize;

            drawNode = new CCDrawNode();
            DrawBackground();
            CreateLabel();
        }

        public string ButtonText
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
            }
        }

        public CCRect GetBoundingBox()
        {
            return new CCRect(Position.X, Position.Y, width, height);
        }

        private void CreateLabel()
        {
            this.label = new CCLabel(text, fontType, fontSize, CCLabelFormat.SystemFont);
            label.Color = fontColor;
            label.PositionX = width / 2;
            label.PositionY = height / 2;
            this.AddChild(label);
        }

        private void DrawBackground()
        {
            drawNode.DrawRect(new CCRect(0, 0, width, height), backColor, borderWidth, borderColor);
            this.AddChild(drawNode);
        }
    }
}
