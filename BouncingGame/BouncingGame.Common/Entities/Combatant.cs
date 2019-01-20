﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CocosSharp;
using Newtonsoft.Json.Linq;
using static SpellDefense.Common.GodClass;

namespace SpellDefense.Common.Entities
{
    public abstract class Combatant : GamePiece
    {
        private List<CardAct> abilityList = new List<CardAct>();
        //Special function pointer, for ranged units
        public Action<Projectile> AddProjectile;
        public float drawSize;
        protected Boolean meleeUnit;
        double timeUntilAttack;
        public int attackPwr { get; set; }
        public GamePiece defaultEnemy;
        GamePiece attackTarget;
        public double moveSpeed { get; set; }
        protected double attackSpeed { get; set; }
        public CCDrawNode targetLine;
        string spriteImage { get; set; }


        //How long are this combatant's arms? Glad you asked...
        protected double attackRange { get; set; }
        protected double aggroRange { get; set; }
        
        public Combatant(TeamColor teamColor) : base(teamColor)
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
            if (GodClass.debug)
            {
                DrawTargetLine();
            }
        }

        private void EngageTarget()
        {
            //If attack range of 1, check for collision between targets
            if (attackRange <= 1 && CheckCollision(this, attackTarget))
            {
                AttackEnemy(attackTarget);
                state = State.attacking;
            }
            else if (distanceTo(this, attackTarget) <= attackRange)
            {
                AttackEnemy(attackTarget);
                state = State.attacking;
            }
            else
            {
                state = State.walking;
            }
        }
        
        private bool CheckCollision(Combatant c1, GamePiece c2)
        {
            return c1.BoundingBox.IntersectsRect(c2.BoundingBox);
        }

        public void MovePhase(float frameTimeInSeconds)
        {
            if (this.state == State.walking)
            {
                double diffX = attackTarget.Position.X - Position.X;
                double diffY = attackTarget.Position.Y - Position.Y;
                double length = Math.Sqrt(diffX * diffX + diffY * diffY); //Pythagorean law
                float dx = (float)(diffX / length * moveSpeed * frameTimeInSeconds); //higher speed is faster
                float dy = (float)(diffY / length * moveSpeed * frameTimeInSeconds);

                this.Position += new CCPoint(dx, dy);

            }
        }

        protected void DrawHealthBar()
        {
            float barHeight = this.drawSize * .2f;
            float currentBarWidth = (float)(this.drawSize * (this.currentHealth / this.maxHealth));

            var greenHealth = new CCRect(-this.drawSize/2, this.drawSize/2 + barHeight, currentBarWidth, barHeight);
            drawNode.DrawRect(greenHealth, fillColor: CCColor4B.Green);
        }

        public void InitFromJSON(String text)
        {
            JObject testJson = JObject.Parse(text);
            attackSpeed = (double)testJson["attackSpeed"];
            moveSpeed = (double)testJson["moveSpeed"];
            spriteImage = (string)testJson["spriteImage"];
            attackPwr = (int)testJson["attackPwr"];
            maxHealth = (int)testJson["maxHealth"];
            attackRange = (int)testJson["attackRange"];
            aggroRange = (int)testJson["aggroRange"];
            JArray abilities = (JArray)testJson["abilities"];

            foreach (JObject ability in abilities)
            {
                string abilityName = (string)ability["actionName"];
                JObject compileTimeArgs = (JObject)ability["compileTimeArgs"];
                CardAct tempAction = GodClass.GetAction(abilityName, compileTimeArgs);
                this.abilityList.Add(tempAction);
            }
        }

    }
}
