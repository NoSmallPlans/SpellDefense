using CocosSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SpellDefense.Common.GodClass;

namespace SpellDefense.Common.Entities
{
    public class TurnManager
    {
        private CCLabel TurnCountDownLabel;
        public EventHandler OnTurnTimeReached;
        float timeInBetweenTurns;
        float timeUntilNextTurn;
        float timeSinceLastTurn;

        public TurnManager(TeamColor teamColor,int timeBetweenTurns)
        {
            this.timeInBetweenTurns = timeBetweenTurns;
            timeUntilNextTurn = timeInBetweenTurns;
            timeSinceLastTurn = timeInBetweenTurns;
            TurnCountDownLabel = new CCLabel(timeUntilNextTurn.ToString(), "Arial", 30, CCLabelFormat.SystemFont);
            if (teamColor == TeamColor.RED)
            {
                TurnCountDownLabel.PositionX = gameplayLayer.ContentSize.Width * 0.15f;
                TurnCountDownLabel.PositionY = gameplayLayer.ContentSize.Height * 0.85f;
            }
            else
            {
                TurnCountDownLabel.PositionX = gameplayLayer.ContentSize.Width * 0.85f;
                TurnCountDownLabel.PositionY = gameplayLayer.ContentSize.Height * 0.85f;
            }
            TurnCountDownLabel.Color = CCColor3B.White;
            GodClass.hudLayer.AddChild(TurnCountDownLabel);
        }

        public CCLabel GetTurnCountDownLabel()
        {
            return TurnCountDownLabel;
        }

        public void Activity(float frameTime)
        {
            timeSinceLastTurn += frameTime;
            timeUntilNextTurn = timeInBetweenTurns - timeSinceLastTurn;
            TurnCountDownLabel.Text = ((int)timeUntilNextTurn).ToString();
            if (timeSinceLastTurn > timeInBetweenTurns)
            {
                timeSinceLastTurn = 0;
                if (OnTurnTimeReached != null)
                {
                    OnTurnTimeReached(typeof(TurnManager), EventArgs.Empty);
                }
            }
        }
    }
}
