using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpellDefense.Common.Entities
{
    public class CardAct
    {

        private Delegate act;

        private int numReqArgs;

        public CardAct(Delegate action, int numReqArguments = 0)
        {
            this.act = action;
            this.numReqArgs = numReqArguments;
        }

        public int GetNumReqArgs() { return this.numReqArgs; }

        public void perform(int[] inputArgs)
        {
            if (inputArgs.Length != numReqArgs) throw new Exception("Missing gameTime arguments for action");

            this.act.DynamicInvoke(inputArgs);
        }

        public void perform()
        {
            this.act.DynamicInvoke();
        }

    }
}
