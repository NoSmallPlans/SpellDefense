using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CocosSharp;
using Newtonsoft.Json.Linq;
using SpellDefense.Common.Actions;
using static SpellDefense.Common.GodClass;

namespace SpellDefense.Common.Entities
{
    public abstract class Combatant : GamePiece
    {
        private List<CardAct> abilityList = new List<CardAct>();
        //Special function pointer, for ranged units
        public Action<Projectile> AddProjectile;
        public float drawSize;
        protected Boolean shootsProjectile;
        double timeUntilAttack;
        public int attackPwr { get; set; }
        public GamePiece defaultEnemy;
        GamePiece attackTarget;
        public double moveSpeed { get; set; }
        protected double attackSpeed { get; set; }
        public CCDrawNode targetLine;
        string fillColorName { get; set; }
        CCColor4B drawColor;
        protected CCSprite combatSprite;
        protected string projectileSpriteName;
        protected float projectileSpeed;
        CCAction walkAction, attackAction, deathAction, hurtAction, idleAction;

        //How long are this combatant's arms? Glad you asked...
        protected double attackRange { get; set; }
        protected double aggroRange { get; set; }

        protected override void ActionStateChanged(ActionState newState)
        {
            if (combatSprite != null && State != newState)
            {
                combatSprite.StopAllActions();
                switch (newState)
                {
                    case ActionState.attacking:
                        combatSprite.RunActions((CCFiniteTimeAction)attackAction, (CCFiniteTimeAction)idleAction);
                        break;
                    case ActionState.waiting:
                        combatSprite.RunAction(idleAction);
                        break;
                    case ActionState.walking:
                        combatSprite.RunAction(walkAction);
                        break;
                    case ActionState.dead:
                        combatSprite.RunActionsAsync((CCFiniteTimeAction)deathAction, new CCFadeOut(1.5f));
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

        private void DrawTargetLine()
        {
            targetLine.Clear();
            targetLine.DrawLine(this.Position, this.attackTarget.Position, CCColor4B.Green, CCLineCap.Square);
        }

        private void AttackEnemy(GamePiece enemy)
        {
            if (enemy != null && this.timeUntilAttack <= 0)
            {
                combatSprite.RunAction(attackAction);
                if (!shootsProjectile)
                {
                    enemy.UpdateHealth(-this.attackPwr);
                }
                else
                {
                    CreateProjectile();
                }

                if (enemy.currentHealth <= 0)
                {
                    this.State = ActionState.walking;
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
            if (distanceTo(this, attackTarget) <= attackRange)
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
            attackPwr = (int)testJson["attackPwr"];
            maxHealth = (int)testJson["maxHealth"];
            attackRange = (int)testJson["attackRange"];
            aggroRange = (int)testJson["aggroRange"];
            if (testJson.ContainsKey("shootsProjectile"))
            {
                shootsProjectile = (string)testJson["shootsProjectile"] == "true" ? true : false;
            }
            else
                shootsProjectile = false;
            if (testJson.ContainsKey("projSprite")){
                projectileSpriteName = (string)testJson["projSprite"];
            }
            if (testJson.ContainsKey("projSpeed")){
                projectileSpeed = (float)testJson["projSpeed"];
            }
            if (testJson.ContainsKey("fillColor")) {
                fillColorName = (string)testJson["fillColor"];
                drawColor = ConvertStringToColor(fillColorName);
            }
            if(testJson.ContainsKey("sprite")) {
                InitSpriteFromJSON((string)testJson["sprite"]);
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

        private void InitSpriteFromJSON(string name)
        {
            CCSpriteSheet spriteSheet = new CCSpriteSheet(name + ".plist", name + ".png");

            var animFrames = spriteSheet.Frames.FindAll(item => item.TextureFilename.ToLower().Contains("idle"));
            idleAction = new CCRepeatForever(new CCAnimate(new CCAnimation(animFrames, 1.3f)));
            combatSprite = new CCSprite(animFrames[0]);
            combatSprite.AddAction(idleAction);

            animFrames = spriteSheet.Frames.FindAll(item => item.TextureFilename.ToLower().Contains("move"));
            walkAction = new CCRepeatForever(new CCAnimate(new CCAnimation(animFrames, 0.3f)));
            combatSprite.AddAction(walkAction);

            animFrames = spriteSheet.Frames.FindAll(item => item.TextureFilename.ToLower().Contains("hurt"));
            hurtAction = new CCRepeat(new CCAnimate(new CCAnimation(animFrames, 0.1f)), 1);
            combatSprite.AddAction(hurtAction);

            animFrames = spriteSheet.Frames.FindAll(item => item.TextureFilename.ToLower().Contains("attack"));
            attackAction = new CCRepeat(new CCAnimate(new CCAnimation(animFrames, 0.1f)), 1);
            combatSprite.AddAction(attackAction);

            animFrames = spriteSheet.Frames.FindAll(item => item.TextureFilename.ToLower().Contains("die"));
            deathAction = new CCRepeat(new CCAnimate(new CCAnimation(animFrames, 0.2f)), 1);
            combatSprite.AddAction(deathAction);

            combatSprite.Scale = 2f;
            this.ContentSize = combatSprite.ScaledContentSize;
            if(teamColor == TeamColor.RED)
            {
                combatSprite.FlipX = true;
            }
            
            this.AddChild(combatSprite);
            combatSprite.RunAction(walkAction);
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
            ContentSize = new CCSize(this.drawSize, this.drawSize);
            if (this.teamColor == TeamColor.RED)
            {
                drawNode.DrawRect(
                    p: CCPoint.Zero,
                    size: this.drawSize,
                    color: drawColor);
            }
            else
            {
                //drawNode.AnchorPoint = new CCPoint(0, 0);
                //drawNode.Position = new CCPoint(-drawSize / 2, -drawSize / 2);
                CCV3F_C4B pt1 = new CCV3F_C4B(new CCPoint(drawSize, 0), drawColor);
                CCV3F_C4B pt2 = new CCV3F_C4B(new CCPoint(0, 0), drawColor);
                CCV3F_C4B pt3 = new CCV3F_C4B(new CCPoint(drawSize/2, drawSize), drawColor);
                CCV3F_C4B[] ptArray = { pt1, pt2, pt3 };
                drawNode.DrawTriangleList(ptArray);
            }
            DrawHealthBar();
        }

    }
}
