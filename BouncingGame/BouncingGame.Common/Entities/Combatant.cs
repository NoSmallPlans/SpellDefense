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
        public CCDrawNode targetLine;


        //How long are this combatant's arms? Glad you asked...
        protected float attackRange;
        protected float aggroRange;
        
        public Combatant(Team.ColorChoice teamColor) : base(teamColor)
        {
            state = State.walking;
            aggroRange = 64;
            attackRange = 16;
            targetLine = new CCDrawNode();
        }

        private void DrawTargetLine()
        {
            targetLine.Clear();
            targetLine.DrawLine(this.Position, this.attackTarget.Position, CCColor4B.Green, CCLineCap.Square);
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
            float distToEnemy;
            GamePiece currentTarget = defaultEnemy;
            float distToTarget = distanceTo(this, currentTarget);
            foreach(Combatant enemy in enemyList)
            {
                distToEnemy = distanceTo(this, enemy);

                if (distToEnemy <= this.aggroRange && distToEnemy < distToTarget)
                {
                    distToTarget = distToEnemy;
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
            diffY = c1.PositionY - c2.PositionY;
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

            //Debugging line to help with targeting
            if (GameCoefficients.debug)
            {
                DrawTargetLine();
            }
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
