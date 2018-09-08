using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CocosSharp;

namespace SpellDefense.Common.Entities
{
    public abstract class Combatant : CCNode
    {
        protected CCDrawNode drawNode;
        CCDrawNode debugGrahic;
        CCLabel extraPointsLabel;
        float timeUntilAttack;

        public CCPoint Speed;


        //How long are this combatant's arms? Glad you asked...
        private float AttackRadius;

        public GameCoefficients.TeamColor teamColor
        {
            get;
            private set;
        }


        public Combatant(GameCoefficients.TeamColor teamColor)
        {
            this.teamColor = teamColor;

            //CreateCollision();
        }

        public abstract void CreateCombatantGraphic();

        public abstract void CreateCollision();

        public void Activity(float frameTimeInSeconds)
        {
            timeUntilAttack -= frameTimeInSeconds;

            if(teamColor == GameCoefficients.TeamColor.RED)
                this.Position += Speed * frameTimeInSeconds;
            else if (teamColor == GameCoefficients.TeamColor.BLUE)
                this.Position -= Speed * frameTimeInSeconds;

        }
    }
}
