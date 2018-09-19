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

        public Base(Team.ColorChoice teamColor) : base(teamColor)
        {
            maxHealth = 1000;
            currentHealth = maxHealth;

            sprite = new CCSprite("CastleGreen");
            sprite.Scale = 0.3f;
            float spriteWidth = sprite.ContentSize.Width * sprite.ScaleX / 4;
            if (teamColor == Team.ColorChoice.RED)
            {
                sprite.RotationY = 180;
                this.Position = new CCPoint(-spriteWidth, GameCoefficients.BattlefieldDimensions.GetHeight() / 2);                
            }
            else
            {
                this.Position = new CCPoint(GameCoefficients.BattlefieldDimensions.GetWidth() + spriteWidth, GameCoefficients.BattlefieldDimensions.GetHeight() / 2);
            }

            this.AddChild(sprite);
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
