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

        public override void CreateProjectile()
        {
            throw new NotImplementedException();
        }



    }
}
