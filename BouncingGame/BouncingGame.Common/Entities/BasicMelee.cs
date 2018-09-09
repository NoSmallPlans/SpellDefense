﻿using CocosSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpellDefense.Common.Entities
{
    class BasicMelee : Combatant
    {
        public int drawSize;
        public BasicMelee(GameCoefficients.TeamColor teamColor) : base(teamColor)
        {
            this.drawSize = 20;
            this.Speed = new CCPoint(10,0);
            this.currentHealth = 100;
            this.maxHealth = 100;
            this.meleeAttackPwr = 5;
            this.attackSpeed = 10;

            //this last... ALWAYS!
            this.CreateCombatantGraphic();
        }

        public override void CreateCollision()
        {
            throw new NotImplementedException();
        }

        public override void CreateCombatantGraphic()
        {
            CCColor4B team;

            if (this.teamColor == GameCoefficients.TeamColor.RED)
            {
                team = CCColor4B.Red;
            }
            else
            {
                team = CCColor4B.Blue;
            }

            drawNode = new CCDrawNode();
            this.AddChild(drawNode);

            drawNode.DrawRect(
                p: CCPoint.Zero,
                size: this.drawSize,
                color: team);

            float barHeight = this.drawSize * .2f;
            float currentBarWidth = this.drawSize * (this.currentHealth / this.maxHealth);

            var greenHealth = new CCRect(-this.drawSize / 2, this.drawSize*0.5f + barHeight, currentBarWidth, barHeight);
            drawNode.DrawRect(greenHealth, fillColor: CCColor4B.Green);

        }



    }
}
