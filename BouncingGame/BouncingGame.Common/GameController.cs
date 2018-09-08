using CocosSharp;
using SpellDefense.Common.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpellDefense.Common
{
    public static class GameController
    {
        public static CCGameView GameView
        {
            get;
            private set;
        }

        public static void Initialize(CCGameView gameView)
        {
            GameView = gameView;

            //var contentSearchPaths = new List<string>() { "Fonts", "Sounds" };

#if __IOS__
            //contentSearchPaths.Add("Sounds/iOS/");

#else // android
            //contentSearchPaths.Add("Sounds/Android/");


#endif

            //contentSearchPaths.Add("Images");
            //GameView.ContentManager.SearchPaths = contentSearchPaths;

            // We use a lower-resolution display to get a pixellated appearance
            int width = 800;
            int height = 480;
            GameView.DesignResolution = new CCSizeI(width, height);

            //InitializeAudio();

            var scene = new TitleScene(GameView);
            GameView.RunWithScene(scene);
        }

        private static void InitializeAudio()
        {
            CCAudioEngine.SharedEngine.PlayBackgroundMusic("FruityFallsSong");
        }

        public static void GoToScene(CCScene scene)
        {
            GameView.Director.ReplaceScene(scene);
        }
    }
}



