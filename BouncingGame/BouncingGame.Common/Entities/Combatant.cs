using System;
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
        protected GamePiece attackTarget;
        private CCPoint nextMove;
        public string direction;
        public double moveSpeed { get; set; }
        protected double attackSpeed { get; set; }
        protected double armor { get; set; }
        public CCDrawNode targetLine;
        string spriteImage { get; set; }
        string colorName { get; set; }
        string unitType { get; set; }
        string fillColorName { get; set; }
        CCColor4B drawColor;
        AnimationManager animManager;

        //How long are this combatant's arms? Glad you asked...
        protected double attackRange { get; set; }
        protected double aggroRange { get; set; }

        protected override void ActionStateChanged(ActionState newState)
        {
            if (animManager != null && State != newState)
            {
                animManager.StopActions();
                switch (newState)
                {
                    case ActionState.attacking:
                        animManager.PlayAnims(new string[] { "attack", "idle" });
                        break;
                    case ActionState.waiting:
                        animManager.Play("idle");
                        break;
                    case ActionState.walking:
                        animManager.Play("move");
                        break;
                    case ActionState.dead:
                        animManager.Play("die");
                        this.RunActions(new CCDelayTime(1.5f), new CCRemoveSelf(true));
                        break;
                }
            }
        }

        public Combatant(TeamColor teamColor, string unitStats) : base(teamColor)
        {
            State = ActionState.walking;
            timeUntilAttack = 0;
            targetLine = new CCDrawNode();

            InitFromJSON(unitStats);
        }

        protected abstract void PlayAttackAnimation();

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
                    PlayAttackAnimation();
                    enemy.TakeDamage(this.attackPwr);
                } else {
                    CreateProjectile();
                }

                
                if(enemy.currentHealth <= 0)
                {
                    this.State = ActionState.walking;
                    this.attackTarget = null;
                }
                this.timeUntilAttack = this.attackSpeed;
            }
        }

        public override void TakeDamage(int dmg)
        {
            this.currentHealth -= dmg - armor;
            if (this.currentHealth < 0)
                this.currentHealth = 0;
            UpdateHealthBar();
        }

        public abstract void CreateProjectile();

        public override void Collided(Combatant enemy)
        {
            if (this.attackTarget == null && enemy != null)
            {
                this.attackTarget = enemy;
                this.State = ActionState.attacking;
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
                UpdateNextMove();
            }
        }
        
        private void UpdateNextMove()
        {
            nextMove = GodClass.gridManager.FindNextMove(this.gridPos, attackTarget.gridPos);
            //set the direction the player is facing, used for animations
            direction = SetDirection(nextMove);
            //Convert to from tile pos to screen pos
            nextMove = GodClass.gridManager.GetScreenPosFromTilePos(nextMove);
        }

        private string SetDirection(CCPoint dest)
        {
            if(dest.X > gridPos.X)
            {
                if (dest.Y > gridPos.Y)
                    return "ne";
                else if (dest.Y < gridPos.Y)
                    return "se";
                else if (dest.Y == gridPos.Y)
                    return "e";
            }
            else if(dest.X < gridPos.X)
            {
                if (dest.Y > gridPos.Y)
                    return "nw";
                else if (dest.Y < gridPos.Y)
                    return "sw";
                else if (dest.Y == gridPos.Y)
                    return "w";
            }
            else if(dest.X == gridPos.X)
            {
                if (dest.Y > gridPos.Y)
                    return "n";
                else
                    return "s";
            }
            return "n";
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
                if (State != ActionState.attacking)
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
            if (distanceTo(this, attackTarget) - this.radius - attackTarget.radius <= attackRange)
            {
                AttackEnemy(attackTarget);
                State = ActionState.attacking;
            }
            else
            {
                State = ActionState.walking;
            }
        }
        
        private bool CheckCollision(Combatant c1, GamePiece c2)
        {
            return c1.BoundingBox.IntersectsRect(c2.BoundingBox);
        }

        public void MovePhase(float frameTimeInSeconds)
        {
            if (this.State == ActionState.walking)
            {
                double diffX = nextMove.X - Position.X;
                double diffY = nextMove.Y - Position.Y;
                double length = Math.Sqrt(diffX * diffX + diffY * diffY); //Pythagorean law
                float dx = (float)(diffX / length * moveSpeed * frameTimeInSeconds); //higher speed is faster
                float dy = (float)(diffY / length * moveSpeed * frameTimeInSeconds);

                this.Position += new CCPoint(dx, dy);
                UpdateNextMove();
                if (CCPoint.Distance(this.Position, nextMove) < 5)
                {
                    this.gridPos = GodClass.gridManager.GetTileFromScreenTouch(nextMove);
                    UpdateNextMove();
                }
            }
        }

        protected void DrawHealthBar()
        {
            float barHeight = this.drawSize * .2f;
            float healthPercent = (float)(this.currentHealth / this.maxHealth);
            float currentBarWidth = this.drawSize * healthPercent;

            var blackBack = new CCRect(-this.drawSize / 2, this.drawSize / 2 + barHeight, this.drawSize, barHeight);
            var greenHealth = new CCRect(-this.drawSize/2, this.drawSize/2 + barHeight, currentBarWidth, barHeight);
            drawNode.DrawRect(blackBack, fillColor: CCColor4B.Black);
            if(healthPercent > 0.65f)
                drawNode.DrawRect(greenHealth, fillColor: CCColor4B.Green);
            else if(healthPercent > 0.3f)
                drawNode.DrawRect(greenHealth, fillColor: CCColor4B.Yellow);
            else
                drawNode.DrawRect(greenHealth, fillColor: CCColor4B.Red);
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
            colorName = (string)testJson["color"];
            if(testJson.ContainsKey("armor")) {
                armor = (double)testJson["armor"];
            }
            else {
                armor = 0;
            }
            /*
            if (testJson.ContainsKey("fillColor"))
            {
                fillColorName = (string)testJson["fillColor"];
                drawColor = ConvertStringToColor(fillColorName);
            }
            */
            if (testJson.ContainsKey("sprite"))
            {
                string spriteName = (string)testJson["sprite"];
                if (testJson.ContainsKey("animations"))
                {
                    animManager = new AnimationManager(spriteName, testJson);
                }
                else
                {
                    animManager = new AnimationManager(spriteName);
                }
                this.AddChild(animManager);
                animManager.Play("move");
            }
            if (testJson.ContainsKey("abilities"))
            {
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
        
        private CCColor4B ConvertStringToColor(string color)
        {
            string[] rgb = color.Split(',');
            float r = float.Parse(rgb[0]);
            float g = float.Parse(rgb[1]);
            float b = float.Parse(rgb[2]);
            return new CCColor4B(r,g,b, 255);
        }

        public override void CreateGraphic()
        {
            //ContentSize = new CCSize(this.drawSize, this.drawSize);
            /*
            if (this.teamColor == TeamColor.RED)
            {
                drawNode.DrawRect(
                    p: CCPoint.Zero,
                    size: this.drawSize,
                    color: drawColor);
            }
            else
            {
                CCV3F_C4B pt1 = new CCV3F_C4B(new CCPoint(drawSize, 0), drawColor);
                CCV3F_C4B pt2 = new CCV3F_C4B(new CCPoint(0, 0), drawColor);
                CCV3F_C4B pt3 = new CCV3F_C4B(new CCPoint(drawSize/2, drawSize), drawColor);
                CCV3F_C4B[] ptArray = { pt1, pt2, pt3 };
                drawNode.DrawTriangleList(ptArray);
            }
            */
            DrawHealthBar();
        }
        
    }
}
