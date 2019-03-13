using CocosSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpellDefense.Common.Entities
{
    public class RisingText : CCNode
    {
        CCLabel label;

        public RisingText(int amt)
        {
            string text = Math.Abs(amt).ToString();
            label = new CCLabel(text, "Arial", 30, CCLabelFormat.SystemFont);
            label.SystemFontSize = 30;
            if (amt >= 0)
                label.Color = CCColor3B.Green;
            else
                label.Color = CCColor3B.Red;
            this.AddChild(label);

            this.RunActionsAsync(new CCFadeOut(1.0f), new CCMoveTo(1.0f, new CCPoint(this.Position.X, this.Position.Y + 40)));
            this.RunActions(new CCDelayTime(0.5f), new CCRemoveSelf(true));
        }
    }
}
