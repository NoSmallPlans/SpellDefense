using CocosSharp;
using Newtonsoft.Json.Linq;
using SpellDefense.Common.Entities.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SpellDefense.Common.GodClass;

namespace SpellDefense.Common.Entities
{
    public class Team
    {
        public CombatantSpawner combatantSpawner;

        private List<Combatant> combatants;
        private List<Projectile> projectiles;
        int teamBaseMaxHealth;
        private Base teamBase;
        private Base enemyBase;
        CardManager cardManager;
        TeamColor teamColor;
        TurnManager turnManager;

        public Base TeamBase()
        {
            return teamBase;
        }

        public Team(TeamColor teamColor)
        {
            this.teamColor = teamColor;
            combatants = new List<Combatant>();
            projectiles = new List<Projectile>();

            if (GodClass.online)
            {
                if(teamColor == GodClass.clientRef.teamColor)
                {
                    cardManager = new CardManager(teamColor);
                    GodClass.cardHUD.AddChild(cardManager);
                }
            }
            else
            {
                cardManager = new CardManager(teamColor);
                GodClass.cardHUD.AddChild(cardManager);
            }
            CreateCombatantSpawner();
            string className = teamColor == TeamColor.RED ? GodClass.playerOneClass : GodClass.playerTwoClass;
            InitFromJson(GodClass.ClassConfigs[className]);
            this.combatantSpawner.IsSpawning = true;
        }

        private void InitTurnManager(int timeBetweenTurns)
        {
            turnManager = new TurnManager(teamColor, timeBetweenTurns);
            turnManager.OnTurnTimeReached += HandleTurnTimeReached;
            GodClass.hudLayer.AddChild(turnManager.GetTurnCountDownLabel());
        }

        public Action<TeamColor> GameOver;

        public void InitFromJson(String text)
        {
            JObject testJson = JObject.Parse(text);
            if (this.cardManager != null)
            {
                this.cardManager.maxHandSize = (int)testJson["maxHandSize"];
                this.cardManager.maxMana = (int)testJson["maxMana"];
            }
            InitTurnManager((int)testJson["spawnTimer"]);
            this.teamBaseMaxHealth = (int)testJson["baseHealth"];
            JArray startingUnits = (JArray)testJson["startingUnits"];

            foreach (JObject unit in startingUnits)
            {
                string name = (string)unit["name"];
                int count = (int)unit["count"];
                this.combatantSpawner.AddSpawn(count, 0, name);
            }
        }

        public void SetEnemyBase(Base enemyBase)
        {
            this.enemyBase = enemyBase;
        }

        public Base makeBase()
        {
            Base b = new Base(this.teamColor);
            b.maxHealth = this.teamBaseMaxHealth;
            this.teamBase = b;
            return this.teamBase;
        }

        public Base GetBase()
        {
            return this.teamBase;
        }

        public void AddCombatant(Combatant addition, GamePiece defaultEnemy)
        {
            addition.defaultEnemy = defaultEnemy;
            addition.AttackTarget = defaultEnemy;
            addition.AddProjectile += this.AddProjectile;
            this.combatants.Add(addition);
        }

        public List<Combatant> GetCombatants()
        {
            return this.combatants;
        }

        public void Cleanup()
        {
            for (int i = combatants.Count - 1; i >= 0; i--)
            {
                if (combatants[i].currentHealth <= 0)
                {
                    DestroyCombatant(combatants[i], combatants);
                }
            }

            for (int i = projectiles.Count - 1; i >= 0; i--)
            {
                if (projectiles[i].dmg == 0)
                {
                    DestroyProjectile(projectiles[i], projectiles);
                }
            }
        }

        public void AddProjectile(Projectile projectile)
        {
            this.projectiles.Add(projectile);
        }

        public void AttackPhase(float frameTimeInSeconds, List<Combatant> enemies, GamePiece enemyBase)
        {
            foreach(Combatant combatant in combatants)
            {
                combatant.AttackPhase(frameTimeInSeconds, enemies, enemyBase);
            }

            foreach (Projectile projectile in projectiles)
            {
                projectile.update(frameTimeInSeconds);
            }
        }

        public void MovePhase(float frameTimeInSeconds)
        {
            foreach (Combatant combatant in combatants)
            {
                combatant.MovePhase(frameTimeInSeconds);
            }
        }

        private void DestroyCombatant(Combatant combatant, List<Combatant> list)
        {
            combatant.State = GamePiece.ActionState.dead;
            list.Remove(combatant);
        }

        private void DestroyProjectile(Projectile projectile, List<Projectile> list)
        {
            projectile.RemoveFromParent();
            list.Remove(projectile);
        }

        public void CreateCombatantSpawner()
        {
            combatantSpawner = new CombatantSpawner(teamColor);
            combatantSpawner.CombatantSpawned += HandleCombatantSpawned;
            combatantSpawner.CreateSpawnPts();
        }

        private void HandleCombatantSpawned(Combatant combatant)
        {
            this.AddCombatant(combatant, enemyBase); 
            //GodClass.battlefield.AddChild(combatant);
        }

        public void TurnTimerPhase(float frameTimeInSeconds)
        {
            turnManager.Activity(frameTimeInSeconds);
        }

        public void HandleTurnTimeReached(object sender, EventArgs e)
        {
            if(cardManager != null)
                cardManager.NewTurn();
            this.combatantSpawner.HandleTurnTimeReached();
        }
    }
}
