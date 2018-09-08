using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpellDefense.Common
{
    public static class GameCoefficients
    {

        public const float StartingCombatantPerSecond = .1f;

        // This variable controls how many seconds must pass
        // before another combatant-per-second is added. For example, 
        // if the game initially spawns one combatant per 5 seconds, then 
        // the spawn rate is .2 combatant per second. If this value is 60, that
        // means that after 1 minute, the spawn rate will be 1.2 combatant per second.
        // Initial playtesting suggest that this value should be fairly large like 3+ 
        // minutes (180 seconds) or else the game gets hard 
        public const float TimeForExtraCombatantPerSecond = 6 * 60;

        public enum TeamColor { RED, BLUE };
    }

}
