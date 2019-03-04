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
        CCDrawNode attackDrawNode;

        public BasicMelee(TeamColor teamColor, string unitStats) : base(teamColor, unitStats)
        {
            this.drawSize = 20;
            this.currentHealth = this.maxHealth;
            this.meleeUnit = true;
            InitDraw();
        }

        private void InitDraw()
        {
            //this last... ALWAYS!
            drawNode = new CCDrawNode();
            this.AddChild(drawNode);
            this.CreateGraphic();
        }

        protected override void PlayAttackAnimation()
        {
            float x = this.PositionWorldspace.X - this.attackTarget.PositionWorldspace.X;
            float y = this.PositionWorldspace.Y - this.attackTarget.PositionWorldspace.Y;
            attackDrawNode = new CCDrawNode();
            attackDrawNode.DrawLine(CCPoint.Zero, new CCPoint(x * -1, y * -1), CCColor4B.Red, CCLineCap.Butt);
            this.AddChild(attackDrawNode);
            attackDrawNode.RunActions(new CCFadeOut(0.5f), new CCRemoveSelf(true));
        }

        public override void CreateProjectile()
        {
            throw new NotImplementedException();
        }



    }
}
