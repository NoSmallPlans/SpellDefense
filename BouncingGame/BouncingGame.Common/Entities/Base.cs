using CocosSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SpellDefense.Common.GodClass;

namespace SpellDefense.Common.Entities
{
    public class Base : GamePiece
    {
        CCSprite sprite;

        public Base(TeamColor teamColor) : base(teamColor)
        {
            maxHealth = 1000;
            currentHealth = maxHealth;
            drawNode = new CCDrawNode();
            this.AddChild(drawNode);
            CreateGraphic();
        }

        public double GetCurrentHealth()
        {
            return this.currentHealth;
        }

        public override void Collided(Combatant enemy)
        {
            throw new NotImplementedException();
        }

        private void CreateCastleSprite()
        {
            sprite = new CCSprite("CastleGreen");
            sprite.Scale = 0.3f;
            float spriteWidth = sprite.ContentSize.Width * sprite.ScaleX / 4;
            if (teamColor == TeamColor.RED)
            {
                sprite.RotationY = 180;
                this.Position = new CCPoint(-spriteWidth, GodClass.BattlefieldDimensions.GetHeight() / 2);
            }
            else
            {
                this.Position = new CCPoint(GodClass.BattlefieldDimensions.GetWidth() + spriteWidth, GodClass.BattlefieldDimensions.GetHeight() / 2);
            }
            this.AddChild(sprite);
        }

        public override void CreateGraphic()
        {
            CreateCastleSprite();
            DrawHealthBar();
            this.ContentSize = sprite.ScaledContentSize;
            drawNode.DrawRect(this.BoundingBox);
        }

        protected void DrawHealthBar()
        {
            float drawSizeWidth = sprite.ScaledContentSize.Width / 2;
            float drawSizeHeight = sprite.ScaledContentSize.Height / 2;
            float barHeight = drawSizeHeight * .2f;
            float currentBarWidth = (float)(drawSizeWidth * (this.currentHealth / this.maxHealth));
            if(teamColor == TeamColor.RED)
            {
                var greenHealth = new CCRect(0.6f*drawSizeWidth, drawSizeHeight+barHeight * 0.5f + barHeight, currentBarWidth, barHeight);
                drawNode.DrawRect(greenHealth, fillColor: CCColor4B.Green);
            } else
            {
                var greenHealth = new CCRect(-1.6f*drawSizeWidth, drawSizeHeight+barHeight * 0.5f + barHeight, currentBarWidth, barHeight);
                drawNode.DrawRect(greenHealth, fillColor: CCColor4B.Green);
            }
            
        }
    }

}
