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
        GamePiece attackTarget = null;
        public CCPoint Speed;
        protected float attackSpeed;


        //How long are this combatant's arms? Glad you asked...
        private float attackRange;
        private float aggroRange;
        
        public Combatant(Team.ColorChoice teamColor) : base(teamColor)
        {
            state = State.walking;
            aggroRange = 32;
            attackRange = 16;
        }

        private void AttackEnemy(GamePiece enemy)
        {
            if (enemy != null && this.timeUntilAttack <= 0)
            {
                enemy.UpdateHealth(-this.meleeAttackPwr);
                if(enemy.currentHealth <= 0)
                {
                    this.state = State.walking;
                    this.attackTarget = null;
                }
                this.timeUntilAttack = this.attackSpeed;
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

        
        public GamePiece FindTarget(List<Combatant> enemyList, GamePiece defaultEnemy)
        {
            float distanceToEnemy;
            GamePiece currentTarget = defaultEnemy;
            float distanceToTarget = distanceTo(this, enemyList[0]);
            foreach(Combatant enemy in enemyList)
            {
                distanceToEnemy = distanceTo(this, enemy);

                if (distanceToEnemy <= this.aggroRange && distanceToEnemy < distanceToTarget)
                {
                    distanceToTarget = distanceToEnemy;
                    currentTarget = enemy;
                }
            }
            return currentTarget;
        }
        
        private float distanceTo(GamePiece c1, GamePiece c2)
        {
            float diffX;
            float diffY;
            diffX = c1.PositionX - c2.PositionX;
            diffY = c2.PositionY - c2.PositionY;
            return (float)Math.Sqrt(diffX * diffX + diffY * diffY);
        }

        public void AttackPhase(float frameTimeInSeconds, List<Combatant> enemies, GamePiece defaultEnemy)
        {
            this.timeUntilAttack -= frameTimeInSeconds;

            if(this.attackTarget != null)
            {
                if (state != State.attacking)
                {
                    attackTarget = FindTarget(enemies, defaultEnemy);
                }
            }
            else
            {
                attackTarget = FindTarget(enemies, defaultEnemy);
            }
            EngageTarget();
        }

        private void EngageTarget()
        {            
            if (distanceTo(this, attackTarget) <= attackRange)
            {
                AttackEnemy(attackTarget);
                state = State.attacking;
            }
            else
            {
                state = State.walking;
            }
        }
        
        public void MovePhase(float frameTimeInSeconds)
        {
            if (this.state == State.walking)
            {
                if (teamColor == Team.ColorChoice.RED)
                    this.Position += Speed * frameTimeInSeconds;
                else if (teamColor == Team.ColorChoice.BLUE)
                    this.Position -= Speed * frameTimeInSeconds;
            }
        }
    }
}
