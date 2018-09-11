using CocosSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpellDefense.Common.Entities
{
    public class Base : CCNode
    {
        CCSprite sprite;
        int maxHealth;
        int currentHealth;
        CCDrawNode healthBar;
        GameCoefficients.TeamColor team;
        int collisionHeight;
        int collisionWidth;


        public Base(GameCoefficients.TeamColor teamColor, CCLayer layer)
        {
            team = teamColor;
            maxHealth = 1000;
            currentHealth = maxHealth;

            sprite = new CCSprite("CastleGreen");
            sprite.Scale = 0.3f;
            float spriteWidth = sprite.ContentSize.Width * sprite.ScaleX / 4;
            if (teamColor == GameCoefficients.TeamColor.RED)
            {
                sprite.RotationY = 180;
                this.Position = new CCPoint(-spriteWidth, GameCoefficients.Battlefield.GetHeight() / 2);                
            }
            else
            {
                this.Position = new CCPoint(layer.ContentSize.Width + spriteWidth, GameCoefficients.Battlefield.GetHeight() / 2);
            }


            this.AddChild(sprite);
            layer.AddChild(this);
        }
    }
}
