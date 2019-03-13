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
        public List<Projectile> projectiles;
        public BasicMelee(TeamColor teamColor, string unitStats) : base(teamColor, unitStats)
        {
            this.drawSize = 20;
            this.currentHealth = this.maxHealth;
            //this.meleeUnit = true;
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

        public override void CreateProjectile()
        {
            Projectile myProjectile;
            if (projectileSpriteName != null)
                myProjectile = new Projectile(this.AttackTarget, this.attackPwr, projectileSpriteName, projectileSpeed);
            else
                myProjectile = new Projectile(this.AttackTarget, this.attackPwr, this.teamColor, projectileSpeed);
            this.Parent.AddChild(myProjectile);
            myProjectile.Position = this.Position;
            AddProjectile(myProjectile);
        }



    }
}
