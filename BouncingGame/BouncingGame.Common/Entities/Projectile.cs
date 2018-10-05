using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CocosSharp;
using static SpellDefense.Common.Entities.Team;

namespace SpellDefense.Common.Entities
{
    class Projectile : CCNode
    {
        public Projectile(GamePiece target, int dmg, ColorChoice teamColor)
        {
            this.dmg = dmg;
            this.target = target;
            this.collisionRect = new CCRect(0, 0, drawSize, drawSize);
            this.teamColor = teamColor;
            this.drawSize = 100;
            CreateGraphic();
        }

        protected CCRect collisionRect;
        protected CCDrawNode drawNode;
        protected GamePiece target;
        protected float moveSpeed;
        protected int dmg;
        protected int drawSize;
        ColorChoice teamColor;

        public void update(float frameTimePerSecond)
        {
            move(frameTimePerSecond);
            //TODO - Fixme
            //Collision checking doesn't work, because not part of layer
            if(checkCollision(this.target))
            {
                this.dealDmg(this.target, this.dmg);
                this.RemoveFromParent();
            }
        }

        public Boolean checkCollision(GamePiece target)
        {
            return collisionRect.IntersectsRect(target.BoundingBox);
        }

        public void dealDmg(GamePiece target, int dmg)
        {
            target.UpdateHealth(-dmg);
        }


        public void move(float frameTimeInSeconds)
        {
            double diffX = target.Position.X - Position.X;
            double diffY = target.Position.Y - Position.Y;
            double length = Math.Sqrt(diffX * diffX + diffY * diffY); //Pythagorean law
            float dx = (float)(diffX / length * moveSpeed * frameTimeInSeconds); //higher speed is faster
            float dy = (float)(diffY / length * moveSpeed * frameTimeInSeconds);

            this.Position += new CCPoint(dx, dy);
        }

        public void CreateGraphic()
        {
            CCColor4B team;

            drawNode = new CCDrawNode();

            if (this.teamColor == Team.ColorChoice.RED)
            {
                team = CCColor4B.Red;
            }
            else
            {
                team = CCColor4B.Blue;
            }

            this.AddChild(drawNode);

            drawNode.DrawCircle(
                center: CCPoint.Zero,
                radius: this.drawSize/2,
                color: team);
            /*
            float barHeight = this.drawSize * .2f;
            float currentBarWidth = this.drawSize * (this.currentHealth / this.maxHealth);

            var greenHealth = new CCRect(-this.drawSize / 2, this.drawSize * 0.5f + barHeight, currentBarWidth, barHeight);
            drawNode.DrawRect(greenHealth, fillColor: CCColor4B.Green);

            if (GameCoefficients.debug)
            {
                drawNode.DrawCircle(this.Position, (int)aggroRange, CCColor4B.Blue);
                drawNode.DrawCircle(this.Position, (int)attackRange, CCColor4B.Orange);
            }
            */
        }

    }
}
