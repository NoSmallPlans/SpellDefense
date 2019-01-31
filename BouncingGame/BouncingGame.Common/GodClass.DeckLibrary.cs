using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpellDefense.Common
{
    public static partial class GodClass
    {
        public static Dictionary<string, string> Decks = new Dictionary<string, string>()
        {
             {
                "deck1",
                @"{
                    'cards': [
                        {
                            'name': 'fireball',
                            'amount': '2'
                        },
                        {
                            'name': 'Haste',
                            'amount': '2'
                        },
                    ]
                }"
            },
             {
                "deck2",
                @"{
                    'cards': [
                        {
                            'name': 'Add Ranged',
                            'amount': '2'
                        },
                        {
                            'name': 'Slow',
                            'amount': '2'
                        },
                    ]
                }"
            }
        };
    }
}
