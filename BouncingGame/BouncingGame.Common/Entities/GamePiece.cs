using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CocosSharp;

namespace SpellDefense.Common.Entities
{

    public abstract class GamePiece : CCNode
    {
        public enum State
        {
            waiting,
            walking,
            attacking,
            dead
        }

        protected CCDrawNode drawNode;
        CCDrawNode debugGrahic;
        protected float currentHealth;
        protected float maxHealth;
        public int collisionWidth;
        public int collisionHeight;

        public State state
        {
            get;
            protected set;
        }

        public GameCoefficients.TeamColor teamColor
        {
            get;
            private set;
        }

        public GamePiece(GameCoefficients.TeamColor teamColor)
        {
            this.teamColor = teamColor;
            state = State.waiting;
        }

        private void UpdateHealthBar()
        {
            drawNode.Clear();
            CreateGraphic();
        }

        public void UpdateHealth(int amt)
        {
            this.currentHealth += amt;
            UpdateHealthBar();
            if (this.currentHealth <= 0)
            {
                this.state = State.dead;
            }
        }

        public abstract void Collided(Combatant enemy);


        public abstract void CreateGraphic();

        public abstract void CreateCollision();

        public abstract void Activity(float frameTimeInSeconds);
    }
}
