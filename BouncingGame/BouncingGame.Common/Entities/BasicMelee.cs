using CocosSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SpellDefense.Common.GodClass;

namespace SpellDefense.Common.Entities
{
    class BasicMelee : Combatant
    {
        public BasicMelee(TeamColor teamColor) : base(teamColor)
        {
            this.drawSize = 20;
            this.speed = 40;
            this.currentHealth = 100;
            this.maxHealth = 100;
            this.attackPwr = 20;
            this.attackSpeed = 2;
            this.meleeUnit = true;

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

            if (this.teamColor == TeamColor.RED)
            {
                team = CCColor4B.Red;
            }
            else
            {
                team = CCColor4B.Blue;
            }
            
            //this.AddChild(drawNode);

            drawNode.DrawRect(
                p: CCPoint.Zero,
                size: this.drawSize,
                color: team);

            DrawHealthBar();

            if (GodClass.debug)
            {
                drawNode.DrawCircle(this.Position, (int)aggroRange, CCColor4B.Blue);
                drawNode.DrawCircle(this.Position, (int)attackRange, CCColor4B.Orange);
            }

        }

        public override void CreateProjectile()
        {
            throw new NotImplementedException();
        }



    }
}
