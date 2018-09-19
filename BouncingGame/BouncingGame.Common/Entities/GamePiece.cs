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
        public float currentHealth
        {
            get;
            protected set;
        }
        public float maxHealth
        {
            get;
            protected set;
        }
        public int collisionWidth;
        public int collisionHeight;

        public State state
        {
            get;
            protected set;
        }

        public Team.ColorChoice teamColor
        {
            get;
            private set;
        }

        public GamePiece(Team.ColorChoice teamColor)
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
        }

        public void Cleanup()
        {
            if (this.currentHealth <= 0)
            {
                this.state = State.dead;
            }
        }

        public abstract void Collided(Combatant enemy);


        public abstract void CreateGraphic();

        public abstract void CreateCollision();

        //public abstract void Activity(float frameTimeInSeconds);
    }
}
