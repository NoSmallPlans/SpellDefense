using CocosSharp;
using SpellDefense.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SpellDefense.Common.Entities.GamePiece;

namespace SpellDefense.Common.Actions
{
    public class CombatantChangeStateAction : CCActionInstant
    {
        readonly ActionState combatantState;

        public CombatantChangeStateAction(ActionState newState)
        {
            combatantState = newState;
        }

        public override CCFiniteTimeAction Reverse()
        {
            throw new NotImplementedException();
        }

        protected override CCActionState StartAction(CCNode target)
        {
            return new CombatantActionState(this, target, combatantState);
        }
    }

    public class CombatantActionState : CCActionInstantState
    {
        readonly Combatant castedTarget;

        public CombatantActionState(CombatantChangeStateAction action, CCNode target, ActionState newState) : base(action, target)
        {
            castedTarget = target as Combatant;

            if (castedTarget == null)
            {
                throw new InvalidOperationException("The argument target must be a Combatant");
            }

            castedTarget.State = newState;
        }
    }
}
