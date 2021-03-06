﻿using System;
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
        private ActionState state;
        public CCPoint gridPos;

        public double currentHealth
        {
            get;
            protected set;
        }

        public double maxHealth
        {
            get;
            set;
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

        public float radius
        {
            get;
            set;
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

        protected void UpdateHealthBar()
        {
            drawNode.Clear();
            CreateGraphic();
        }

        public abstract void TakeDamage(int dmg);

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

        public abstract void Collided(Combatant enemy);


        public abstract void CreateGraphic();

        public object this[string propertyName]
        {
            get { return GetType().GetRuntimeProperty(propertyName).GetValue(this, null); }
            set { GetType().GetRuntimeProperty(propertyName).SetValue(this, value, null); }
        }
    }
}
