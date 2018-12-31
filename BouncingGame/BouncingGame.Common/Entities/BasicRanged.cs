using CocosSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SpellDefense.Common.GodClass;

namespace SpellDefense.Common.Entities
{
    class BasicRanged : Combatant
    {
        public List<Projectile> projectiles;
        public int drawSize;
        public CCNode collisionRect;
        public BasicRanged(TeamColor teamColor) : base(teamColor)
        {
            this.drawSize = 20;
            this.speed = 40;
            this.currentHealth = 100;
            this.maxHealth = 100;
            this.attackPwr = 20;
            this.attackSpeed = 2;
            this.attackRange = base.attackRange * 8;
            this.aggroRange = base.aggroRange * 10;
            this.meleeUnit = false;
            this.projectiles = new List<Projectile>();
            

            CreateCollision();
            //this last... ALWAYS!
            drawNode = new CCDrawNode();
            this.CreateGraphic();
        }

        public override void CreateCollision()
        {
            this.collisionHeight = this.drawSize;
            this.collisionWidth = this.drawSize;
        }

        public override void CreateGraphic()
        {
            CCColor4B team;

            if (this.teamColor == TeamColor.RED)
            {
                team = CCColor4B.Red;
            }
            else
            {
                team = CCColor4B.Blue;
            }

            this.AddChild(drawNode);

            CCV3F_C4B pt1 = new CCV3F_C4B(new CCPoint(0, 0), team);
            CCV3F_C4B pt2 = new CCV3F_C4B(new CCPoint(this.drawSize/2, this.drawSize), team);
            CCV3F_C4B pt3 = new CCV3F_C4B(new CCPoint(this.drawSize, 0), team);
            CCV3F_C4B[] ptArray = { pt1, pt2, pt3 };
            drawNode.DrawTriangleList(ptArray);

            float barHeight = this.drawSize * .2f;
            float currentBarWidth = this.drawSize * (this.currentHealth / this.maxHealth);

            var greenHealth = new CCRect(-this.drawSize / 2, this.drawSize * 0.5f + barHeight, currentBarWidth, barHeight);
            drawNode.DrawRect(greenHealth, fillColor: CCColor4B.Green);

            if (GodClass.debug)
            {
                drawNode.DrawCircle(this.Position, (int)aggroRange, CCColor4B.Blue);
                drawNode.DrawCircle(this.Position, (int)attackRange, CCColor4B.Orange);
            }
        }

        public override void CreateProjectile()
        {
            Projectile myProjectile = new Projectile(this.AttackTarget, this.attackPwr, teamColor);
            this.Parent.AddChild(myProjectile);
            myProjectile.Position = this.Position;
            AddProjectile(myProjectile);
        }

    }
}
