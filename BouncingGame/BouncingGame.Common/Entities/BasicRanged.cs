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
        public BasicRanged(TeamColor teamColor, string unitJson) : base(teamColor, unitJson)
        {
            this.drawSize = 25;
            this.moveSpeed = 40;
            this.currentHealth = 100;
            this.maxHealth = 100;
            this.attackPwr = 20;
            this.attackSpeed = 2;
            this.attackRange = base.attackRange * 8;
            this.aggroRange = base.aggroRange * 10;
            this.meleeUnit = false;
            this.projectiles = new List<Projectile>();
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
            throw new NotImplementedException();
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
