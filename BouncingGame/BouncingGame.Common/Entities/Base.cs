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
            sprite.AnchorPoint = CCPoint.AnchorLowerLeft;
            float spriteWidth = sprite.ScaledContentSize.Width;
            if (teamColor == TeamColor.RED)
            {
                sprite.FlipX = true;
                this.Position = new CCPoint(0, GodClass.BattlefieldDimensions.GetHeight() / 4);
            }
            else
            {
                this.Position = new CCPoint(GodClass.BattlefieldDimensions.GetWidth() - spriteWidth, GodClass.BattlefieldDimensions.GetHeight() / 4);
            }
            this.AddChild(sprite);
        }

        public override void CreateGraphic()
        {
            CreateCastleSprite();
            DrawHealthBar();
            this.ContentSize = sprite.ScaledContentSize;
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
            float drawSizeWidth = GodClass.BattlefieldDimensions.GetWidth() / 4;
            float drawSizeHeight = sprite.ScaledContentSize.Height;
            float barHeight = drawSizeHeight * .05f;
            float currentBarWidth = (float)(drawSizeWidth * (this.currentHealth / this.maxHealth));
            float borderCushion = 0.05f * GodClass.BattlefieldDimensions.GetWidth();
            if (teamColor == TeamColor.RED)
            {
                var greenHealth = new CCRect(borderCushion, drawSizeHeight+barHeight * 0.5f + barHeight, currentBarWidth, barHeight);
                drawNode.DrawRect(greenHealth, fillColor: CCColor4B.Green);
            } else
            {
                var greenHealth = new CCRect((-1*drawSizeWidth)+borderCushion, drawSizeHeight+barHeight * 0.5f + barHeight, currentBarWidth, barHeight);
                drawNode.DrawRect(greenHealth, fillColor: CCColor4B.Green);
            }
            
        }
    }

}
