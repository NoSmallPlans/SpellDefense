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

        public TeamColor teamColor
        {
            get;
            private set;
        }

        public GamePiece(TeamColor teamColor)
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

        public CCRect GetBoundingBox()
        {
            return new CCRect(this.Position.X, this.Position.Y, this.drawNode.BoundingBox.Size.Width, this.drawNode.BoundingBox.Size.Height);
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

        public object this[string propertyName]
        {
            get { return GetType().GetRuntimeProperty(propertyName).GetValue(this, null); }
            set { GetType().GetRuntimeProperty(propertyName).SetValue(this, value, null); }
        }

        //public abstract void Activity(float frameTimeInSeconds);
    }
}
