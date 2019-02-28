using CocosSharp;
using SpellDefense.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpellDefense.Common
{
    public static partial class GodClass
    {
        public static bool debug = false;

        public static CCLayer gameplayLayer;
        public static UIcontainer battlefield;
        public static UIcontainer cardHUD;
        // This variable controls how many seconds must pass
        // before another combatant-per-second is added. For example, 
        // if the game initially spawns one combatant per 5 seconds, then 
        // the spawn rate is .2 combatant per second. If this value is 60, that
        // means that after 1 minute, the spawn rate will be 1.2 combatant per second.
        // Initial playtesting suggest that this value should be fairly large like 3+ 
        // minutes (180 seconds) or else the game gets hard 
        public const float TimeForExtraCombatantPerSecond = 6 * 60;

        public enum TeamColor { RED, BLUE };

        public const int desiredWidth = 1024;
        public const int desiredHeight = 768;

        //Eventually PlayHUD dimensions will be more adaptive
        //we're future proofing with this class
        public static class CardHUDdimensions
        {

            public static float GetMinX()
            {
                return 0;
            }

            public static float GetWidth()
            {
                return desiredWidth;
            }

            public static float GetMinY()
            {
                return 0;
            }

            public static float GetHeight()
            {
                const float MENU_RATIO = 0.15f;
                return MENU_RATIO * desiredHeight;
            }
        }

        //Eventually Battlefield dimensions will be more adaptive
        //we're future proofing with this class
        public static class BattlefieldDimensions
        {

            public static float GetMinX()
            {
                return 0;
            }

            public static float GetWidth()
            {
                return desiredWidth;
            }

            public static float GetMinY()
            {
                return CardHUDdimensions.GetHeight();
            }


            public static float GetHeight()
            {
                return desiredHeight - CardHUDdimensions.GetHeight();
            }

            public static CCRect GetBounds()
            {
                return new CCRect(GetMinX(), GetMinY(), GetWidth(), GetHeight());
            }
        }

    }

}
