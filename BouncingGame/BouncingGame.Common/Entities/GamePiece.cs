using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CocosSharp;
using static SpellDefense.Common.GodClass;

namespace SpellDefense.Common.Entities
{

    public abstract class GamePiece : CCNode
    {
        public enum ActionState
        {
            waiting,
            walking,
            attacking,
            dead
        }

        protected CCDrawNode drawNode = new CCDrawNode();
        CCDrawNode debugGrahic;
        private ActionState state;

        public double currentHealth
        {
            get;
            protected set;
        }
        public double maxHealth
        {
            get;
            protected set;
        }

        public virtual ActionState State
        {
            get
            {
                return this.state;
            }
            set
            {
                ActionStateChanged(value);
                this.state = value;
            }
        }

        protected virtual void ActionStateChanged(ActionState newState)
        {
            return;
        }

        public TeamColor teamColor
        {
            get;
            private set;
        }

        public GamePiece(TeamColor teamColor)
        {
            this.teamColor = teamColor;
            state = ActionState.waiting;
        }

        private void UpdateHealthBar()
        {
            drawNode.Clear();
            CreateGraphic();
        }

        public void UpdateHealth(int amt)
        {
            this.currentHealth += amt;
            if (this.currentHealth < 0)
                this.currentHealth = 0;
            UpdateHealthBar();
        }

        public CCRect GetBoundingBox()
        {
            return new CCRect(this.Position.X, this.Position.Y, this.drawNode.BoundingBox.Size.Width, this.drawNode.BoundingBox.Size.Height);
        }

        public void Cleanup()
        {
            if (this.currentHealth <= 0)
            {
                this.state = ActionState.dead;
            }
        }

        public abstract void Collided(Combatant enemy);


        public abstract void CreateGraphic();

        public object this[string propertyName]
        {
            get { return GetType().GetRuntimeProperty(propertyName).GetValue(this, null); }
            set { GetType().GetRuntimeProperty(propertyName).SetValue(this, value, null); }
        }

        //public abstract void Activity(float frameTimeInSeconds);
    }
}
