using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CocosSharp;

namespace SpellDefense.Common.Entities
{
    public abstract class Combatant : GamePiece
    {

        float timeUntilAttack;
        protected int meleeAttackPwr;
        Combatant attackTarget = null;
        public CCPoint Speed;
        protected float attackSpeed;


        //How long are this combatant's arms? Glad you asked...
        private float AttackRange;
        private float AggroRange;
        
        public Combatant(GameCoefficients.TeamColor teamColor) : base(teamColor)
        {
            state = State.walking;
        }

        private void AttackEnemy(Combatant enemy)
        {
            if (enemy != null)
            {
                enemy.UpdateHealth(-this.meleeAttackPwr);
                if(enemy.currentHealth <= 0)
                {
                    this.state = State.walking;
                    this.attackTarget = null;
                }
            }
            else
            {
                this.state = State.walking;
            }
        }

        public override void Collided(Combatant enemy)
        {
            if (this.attackTarget == null && enemy != null)
            {
                this.attackTarget = enemy;
                this.state = State.attacking;
            }
        }

        public override void Activity(float frameTimeInSeconds)
        {
            this.timeUntilAttack -= frameTimeInSeconds;

            if (this.state == State.walking)
            {
                if (teamColor == GameCoefficients.TeamColor.RED)
                    this.Position += Speed * frameTimeInSeconds;
                else if (teamColor == GameCoefficients.TeamColor.BLUE)
                    this.Position -= Speed * frameTimeInSeconds;
            }
            else if(this.state == State.attacking)
            {
                if(this.timeUntilAttack <= 0)
                {
                    this.AttackEnemy(this.attackTarget);
                    this.timeUntilAttack = this.attackSpeed;
                }
            }

        }
    }
}
