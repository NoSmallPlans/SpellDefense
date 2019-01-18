using CocosSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static SpellDefense.Common.GodClass;

namespace SpellDefense.Common.Entities
{
    class BasicRanged : Combatant
    {
        public List<Projectile> projectiles;
        public BasicRanged(TeamColor teamColor) : base(teamColor)
        {
            this.drawSize = 25;
            this.Radius = this.drawSize / 2;
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
            InitDraw();
        }

        private void InitDraw()
        {            
            //this last... ALWAYS!
            drawNode = new CCDrawNode();
            this.AddChild(drawNode);
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
            this.Radius = this.drawSize / 2;

            if (this.teamColor == TeamColor.RED)
            {
                team = CCColor4B.Red;
            }
            else
            {
                team = CCColor4B.Blue;
            }

            CCV3F_C4B pt1 = new CCV3F_C4B(new CCPoint(-this.Radius, this.Radius*2), team);
            CCV3F_C4B pt2 = new CCV3F_C4B(new CCPoint(0, 0), team);
            CCV3F_C4B pt3 = new CCV3F_C4B(new CCPoint(-this.Radius*2, 0), team);
            CCV3F_C4B[] ptArray = { pt1, pt2, pt3 };
            drawNode.DrawTriangleList(ptArray);

            DrawHealthBar();
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
