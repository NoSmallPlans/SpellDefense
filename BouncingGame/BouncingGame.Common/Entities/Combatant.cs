using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CocosSharp;

namespace SpellDefense.Common.Entities
{
    public abstract class Combatant : CCNode
    {
        public enum State
        {
            walking,
            attacking,
            dead
        }

        protected CCDrawNode drawNode;
        CCDrawNode debugGrahic;
        CCLabel extraPointsLabel;
        float timeUntilAttack;
        protected float currentHealth;
        protected float maxHealth;
        protected int meleeAttackPwr;
        Combatant attackTarget = null;
        public State state
        {
            get;
            protected set;
        }

        public CCPoint Speed;
        protected float attackSpeed;


        //How long are this combatant's arms? Glad you asked...
        private float AttackRadius;

        public GameCoefficients.TeamColor teamColor
        {
            get;
            private set;
        }


        public Combatant(GameCoefficients.TeamColor teamColor)
        {
            this.teamColor = teamColor;
            state = State.walking;
            //CreateCollision();
        }

        public void TakeDmg(int dmg)
        {
            this.currentHealth -= dmg;
            if (this.currentHealth < 0)
            {
                this.state = State.dead;
            }
        }

        private void AttackEnemy(Combatant enemy)
        {
            if (enemy != null)
                enemy.TakeDmg(this.meleeAttackPwr);
        }

        public void Collided(Combatant enemy)
        {
            if (this.attackTarget != null) this.attackTarget = enemy;
            this.state = State.attacking;
        }

       
        public abstract void CreateCombatantGraphic();

        public abstract void CreateCollision();

        public void Activity(float frameTimeInSeconds)
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
