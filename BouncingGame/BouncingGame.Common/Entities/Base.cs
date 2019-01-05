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
            CreateCastleSprite();
            CreateCollision();
            CreateGraphic();
        }

        public override void Collided(Combatant enemy)
        {
            throw new NotImplementedException();
        }

        public override void CreateCollision()
        {
            this.collisionHeight = (int)sprite.ScaledContentSize.Height;
            this.collisionWidth = (int)sprite.ScaledContentSize.Width;
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
            DrawHealthBar();
            DrawCollisionBorder();
        }

        private void DrawCollisionBorder()
        {
            drawNode.DrawRect(new CCRect(0, 0, collisionWidth, collisionHeight));
        }

        protected void DrawHealthBar()
        {
            int drawSizeWidth = (int)sprite.ScaledContentSize.Width / 2;
            int drawSizeHeight = (int)sprite.ScaledContentSize.Height / 2;
            float barHeight = drawSizeHeight * .2f;
            float currentBarWidth = drawSizeWidth * (this.currentHealth / this.maxHealth);

            var greenHealth = new CCRect(drawSizeWidth, drawSizeHeight * 0.5f + barHeight, currentBarWidth, barHeight);
            drawNode.DrawRect(greenHealth, fillColor: CCColor4B.Green);
        }
    }

}
