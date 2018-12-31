using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CocosSharp;
using static SpellDefense.Common.Entities.Team;
using static SpellDefense.Common.GodClass;

namespace SpellDefense.Common.Entities
{
    public class Projectile : CCNode
    {
        public Projectile(GamePiece target, int dmg, TeamColor teamColor)
        {
            this.dmg = dmg;
            this.target = target;
            this.drawSize = 10;
            this.teamColor = teamColor;            
            this.moveSpeed = 60;
            CreateGraphic();
        }

        protected CCDrawNode drawNode;
        protected GamePiece target;
        protected float moveSpeed;
        public int dmg;
        protected int drawSize;
        TeamColor teamColor;

        public void update(float frameTimePerSecond)
        {
            move(frameTimePerSecond);
            if(checkCollision(this.target))
            {
                this.dealDmg(this.target, this.dmg);
                this.dmg = 0;
            }
        }

        public CCRect GetBoundingBox()
        {
            return new CCRect(this.Position.X, this.Position.Y, this.drawNode.BoundingBox.Size.Width, this.drawNode.BoundingBox.Size.Height);
        }

        public Boolean checkCollision(GamePiece target)
        {
            return this.GetBoundingBox().IntersectsRect(target.GetBoundingBox());
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

            if (this.teamColor == TeamColor.RED)
            {
                team = CCColor4B.Red;
            }
            else
            {
                team = CCColor4B.Blue;
            }

            this.AddChild(drawNode);

            
            drawNode.DrawSolidCircle(
                pos: CCPoint.Zero,
                radius: this.drawSize/2,
                color: team);
        }

    }
}
