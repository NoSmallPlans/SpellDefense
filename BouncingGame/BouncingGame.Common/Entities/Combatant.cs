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
        protected Boolean meleeUnit;
        float timeUntilAttack;
        protected int attackPwr;
        public GamePiece defaultEnemy;
        GamePiece attackTarget;
        public float speed;
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
                if (meleeUnit)
                {
                    enemy.UpdateHealth(-this.attackPwr);
                } else {
                    CreateProjectile();
                }

                
                if(enemy.currentHealth <= 0)
                {
                    this.state = State.walking;
                    this.attackTarget = null;
                }
                this.timeUntilAttack = this.attackSpeed;
            }
        }

        public abstract void CreateProjectile();

        public override void Collided(Combatant enemy)
        {
            if (this.attackTarget == null && enemy != null)
            {
                this.attackTarget = enemy;
                this.state = State.attacking;
            }
        }

        public GamePiece AttackTarget
        {
            get
            {
                return this.attackTarget;
            }
            set
            {
                attackTarget = value;
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

            if (!meleeUnit) UpdateProjectiles(frameTimeInSeconds);
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
                double diffX = attackTarget.Position.X - Position.X;
                double diffY = attackTarget.Position.Y - Position.Y;
                double length = Math.Sqrt(diffX * diffX + diffY * diffY); //Pythagorean law
                float dx = (float)(diffX / length * speed * frameTimeInSeconds); //higher speed is faster
                float dy = (float)(diffY / length * speed * frameTimeInSeconds);

                this.Position += new CCPoint(dx, dy);

            }
        }

        protected abstract void UpdateProjectiles(float frameTimeInSeconds);


    }
}
