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
        //static bool isSpawning = true;
        float timeUntilNextTurn;
        float timeSinceLastTurn;

        public TurnManager()
        {
            timeInBetweenTurns = 16;
            timeUntilNextTurn = timeInBetweenTurns;
            timeSinceLastTurn = timeInBetweenTurns;
            TurnCountDownLabel = new CCLabel(timeUntilNextTurn.ToString(), "Arial", 30, CCLabelFormat.SystemFont);
        }

        public CCLabel GetTurnCountDownLabel()
        {
            return TurnCountDownLabel;
        }

        public void UpdateTurnCountDownLabel()
        {
            TurnCountDownLabel.Text = ((int)timeUntilNextTurn).ToString();
        }

        public void Activity(float frameTime)
        {
            timeSinceLastTurn += frameTime;
            timeUntilNextTurn = timeInBetweenTurns - timeSinceLastTurn;

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
