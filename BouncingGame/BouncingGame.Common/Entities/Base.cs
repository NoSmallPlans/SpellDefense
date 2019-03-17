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
            sprite = new CCSprite("SmallCastleGreen");
            sprite.Scale = 0.3f;
            float spriteWidth = sprite.ScaledContentSize.Width;
            if (teamColor == TeamColor.RED)
            {
                sprite.FlipX = true;
                this.Position = new CCPoint(spriteWidth/2, GodClass.BattlefieldDimensions.GetHeight() / 4 + sprite.ScaledContentSize.Height/2);
            }
            else
            {
                this.Position = new CCPoint(GodClass.BattlefieldDimensions.GetWidth() - spriteWidth/2, GodClass.BattlefieldDimensions.GetHeight() / 4 + sprite.ScaledContentSize.Height / 2);
            }
            this.AddChild(sprite);
        }

        public override void CreateGraphic()
        {
            CreateCastleSprite();
            DrawHealthBar();
            this.ContentSize = sprite.ScaledContentSize;
            this.radius = this.ContentSize.Width / 2;
        }

        public override void TakeDamage(int dmg)
        {
            this.currentHealth -= dmg;
            if (this.currentHealth < 0)
                this.currentHealth = 0;
            UpdateHealthBar();
        }

        protected void DrawHealthBar()
        {
            float drawSizeWidth = sprite.ScaledContentSize.Width*2;
            float drawSizeHeight = sprite.ScaledContentSize.Height;
            float barHeight = drawSizeHeight * .05f;
            float currentBarWidth = (float)(drawSizeWidth * (this.currentHealth / this.maxHealth));
            float borderCushion = 0.25f * drawSizeWidth;
            if (teamColor == TeamColor.RED)
            {
                var greenHealth = new CCRect(0, drawSizeHeight / 2 + barHeight, currentBarWidth, barHeight);
                drawNode.DrawRect(greenHealth, fillColor: CCColor4B.Green);
            } else
            {
                var greenHealth = new CCRect(-drawSizeWidth, drawSizeHeight / 2 + barHeight * 0.5f + barHeight, currentBarWidth, barHeight);
                drawNode.DrawRect(greenHealth, fillColor: CCColor4B.Green);                
            }
            
        }
    }

}
