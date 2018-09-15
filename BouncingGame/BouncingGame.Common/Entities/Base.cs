using CocosSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpellDefense.Common.Entities
{
    public class Base : GamePiece
    {
        CCSprite sprite;
        CCDrawNode healthBar;

        public Base(GameCoefficients.TeamColor teamColor, CCLayer layer) : base(teamColor)
        {
            maxHealth = 1000;
            currentHealth = maxHealth;

            sprite = new CCSprite("CastleGreen");
            sprite.Scale = 0.3f;
            float spriteWidth = sprite.ContentSize.Width * sprite.ScaleX / 4;
            if (teamColor == GameCoefficients.TeamColor.RED)
            {
                sprite.RotationY = 180;
                this.Position = new CCPoint(-spriteWidth, GameCoefficients.BattlefieldDimensions.GetHeight() / 2);                
            }
            else
            {
                this.Position = new CCPoint(layer.ContentSize.Width + spriteWidth, GameCoefficients.BattlefieldDimensions.GetHeight() / 2);
            }

            this.AddChild(sprite);
            layer.AddChild(this);
        }

        public override void Activity(float frameTimeInSeconds)
        {
            throw new NotImplementedException();
        }

        public override void Collided(Combatant enemy)
        {
            throw new NotImplementedException();
        }

        public override void CreateCollision()
        {
            throw new NotImplementedException();
        }

        public override void CreateGraphic()
        {
            throw new NotImplementedException();
        }
    }

}
