using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpellDefense.Common
{
    public static partial class GodClass
    {
        public static string playerOneDeck = "deck2";
        public static string playerTwoDeck = "deck1";
        public static Dictionary<string, string> Decks = new Dictionary<string, string>()
        {
             {
                "deck1",
                @"{
                    'cards': [
                        {
                            'name': 'spawn grunt',
                            'count': '2'
                        },
                        {
                            'name': 'spawn soldier',
                            'count': '2'
                        },
                        {
                            'name': 'spawn ranged',
                            'count': '2'
                        },
                        {
                            'name': 'spawn sniper',
                            'count': '2'
                        },
                    ]
                }"
            },
             {
                "deck2",
                @"{
                    'cards': [
                        {
                            'name': 'spawn ranged',
                            'count': '2'
                        },
                        {
                            'name': 'slow',
                            'count': '2'
                        },
                        {
                            'name': 'fireball',
                            'count': '2'
                        },
                        {
                            'name': 'nuke',
                            'count': '2'
                        },
                        {
                            'name': 'spawn soldier',
                            'count': '2'
                        },
                    ]
                }"
            }
        };
    }
}
