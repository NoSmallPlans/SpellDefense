using CocosSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpellDefense.Common.Entities
{
    public class Team
    {
        public enum ColorChoice { RED, BLUE };

        public ColorChoice teamColor
        {
            get;
            protected set;
        }
        private List<Combatant> combatants;
        private List<Projectile> projectiles;
        private Base teamBase;

        public Base TeamBase()
        {
            return teamBase;
        }

        public Team(ColorChoice teamColor)
        {
            this.teamColor = teamColor;
            combatants = new List<Combatant>();
            projectiles = new List<Projectile>();
        }

        public Base makeBase()
        {
            return this.teamBase = new Base(this.teamColor);
        }

        public void AddBase(Base myBase)
        {
            this.teamBase = myBase;
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
                combatants[i].Cleanup();
                if (combatants[i].state == Combatant.State.dead)
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
            combatant.RemoveFromParent();
            list.Remove(combatant);
        }

        private void DestroyProjectile(Projectile projectile, List<Projectile> list)
        {
            projectile.RemoveFromParent();
            list.Remove(projectile);
        }
    }
}
