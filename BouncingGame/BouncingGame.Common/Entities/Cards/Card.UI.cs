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
        CCSprite borderSprite;
        CCPoint originalPosition;

        private void UIInit()
        {
            RenderTextures();
            ColorIconTexture = CCTextureCache.SharedTextureCache.AddImage("GreenIcon.png");
            CardTexture = CCTextureCache.SharedTextureCache.AddImage(cardImage);
            CardName = cardTitle;
            DescriptionText = cardText;
            CostText = "Mana: " + cardCost.ToString();
            UsesRenderTexture = true;
            Opacity = 255;
        }

        public CCPoint OriginalPosition
        {
            get
            {
                return originalPosition;
            }
            set
            {
                originalPosition = value;
            }
        }

        public CardState State
        {
            get
            {
                return state;
            }
            set
            {
                switch(value)
                {
                    case CardState.Rest:
                        RestState();
                        break;
                    case CardState.Selected:
                        SelectedState();
                        break;
                    case CardState.Expanded:
                        ExpandedState();
                        break;
                    default:
                        break;
                }
                state = value;
            }
        }

        private void RestState()
        {
            this.Position = originalPosition;
            Scale = 1.0f;
            this.RemoveChild(borderSprite);
        }

        private void SelectedState()
        {
            this.Position = new CCPoint(originalPosition.X, originalPosition.Y + 100);
            Scale = 1.0f;
            if (borderSprite == null)
            {
                CreateBorderSprite();
            }
            else
            {
                AddChild(borderSprite);
            }
        }

        private void ExpandedState()
        {
            Scale = 2.0f;
            Position = new CCPoint(originalPosition.X, originalPosition.Y + 300);
        }

        public CCRect GetBoundingBox()
        {
            return new CCRect(this.Position.X, this.Position.Y, this.renderTexture.Sprite.BoundingBox.Size.Width, this.renderTexture.Sprite.BoundingBox.Size.Height);
        }

        private void CreateBorderSprite()
        {
            borderSprite = new CCSprite("CardHighlight.png");
            borderSprite.AnchorPoint = CCPoint.Zero;
            this.AddChild(borderSprite);
        }
    }
}