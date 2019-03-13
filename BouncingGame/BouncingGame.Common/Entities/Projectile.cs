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
        CCSprite projSprite;
        protected CCDrawNode drawNode;
        protected GamePiece target;
        protected float moveSpeed;
        public int dmg;
        protected float drawSize;
        TeamColor teamColor;
        CCAction moveAction;
        CCSpriteSheet spriteSheet;

        public Projectile(GamePiece target, int dmg, TeamColor teamColor, float speed)
        {
            this.dmg = dmg;
            this.target = target;
            this.drawSize = 10;
            this.teamColor = teamColor;            
            this.moveSpeed = speed;
            CreateGraphic();
        }

        public Projectile(GamePiece target, int dmg, string spriteName, float speed, bool flipX)
        {
            this.dmg = dmg;
            this.target = target;
            this.drawSize = 10;
            this.moveSpeed = speed;
            AddSprite(spriteName, flipX);
        }

        public void update(float frameTimePerSecond)
        {
            move(frameTimePerSecond);
            if(checkCollision(this.target))
            {
                this.dealDmg(this.target, this.dmg);
                this.dmg = 0;
            }
        }

        public Boolean checkCollision(GamePiece target)
        {
            return (CCPoint.Distance(this.Position, target.Position) < this.drawSize);
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

        private void AddSprite(string spriteName, bool flipX)
        {
            spriteSheet = new CCSpriteSheet(spriteName + ".plist", spriteName + ".png");

            var animFrames = spriteSheet.Frames;
            moveAction = new CCRepeatForever(new CCAnimate(new CCAnimation(animFrames, 0.1f)));
            projSprite = new CCSprite(animFrames[0]);
            projSprite.AddAction(moveAction);
            projSprite.FlipX = flipX;
            this.AddChild(projSprite);

            projSprite.RunAction(moveAction);

            this.ContentSize = projSprite.ContentSize;
            this.drawSize = this.ContentSize.Width;
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
